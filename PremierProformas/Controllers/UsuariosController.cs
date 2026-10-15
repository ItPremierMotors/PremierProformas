using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PremierProformas.Data;
using PremierProformas.Models;
using PremierProformas.ViewModels;

namespace PremierProformas.Controllers;

[Authorize(Roles = "Admin")]
public class UsuariosController(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    ApplicationDbContext db) : Controller
{
    private static readonly string[] RolesPermitidos = ["Admin", "JefeVentas", "Vendedor"];

    public async Task<IActionResult> Index()
    {
        var users = await userManager.Users.OrderBy(user => user.Email).ToListAsync();
        var sellers = await db.Vendedores
            .Include(vendedor => vendedor.Sucursal)
            .Where(vendedor => vendedor.UsuarioId != null)
            .ToDictionaryAsync(vendedor => vendedor.UsuarioId!);

        var model = new List<UsuarioListItemViewModel>();
        foreach (var user in users)
        {
            sellers.TryGetValue(user.Id, out var seller);
            model.Add(new UsuarioListItemViewModel
            {
                Id = user.Id,
                Email = user.Email ?? user.UserName ?? "",
                Roles = string.Join(", ", await userManager.GetRolesAsync(user)),
                Vendedor = seller?.NombreCompleto,
                Sucursal = seller?.Sucursal.Nombre,
                Activo = user.LockoutEnd != DateTimeOffset.MaxValue
            });
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        await PrepararListasAsync();
        return View(new CrearUsuarioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(CrearUsuarioViewModel model)
    {
        var esRolValido = RolesPermitidos.Contains(model.Role, StringComparer.Ordinal);
        if (!esRolValido || !await roleManager.RoleExistsAsync(model.Role))
        {
            ModelState.AddModelError(nameof(model.Role), "El rol seleccionado no es válido.");
        }

        var seleccionoVendedor = model.VendedorId.HasValue;
        var ingresoNuevoVendedor = !string.IsNullOrWhiteSpace(model.NombreVendedor) || model.SucursalId.HasValue;
        if (seleccionoVendedor && ingresoNuevoVendedor)
        {
            ModelState.AddModelError(nameof(model.VendedorId), "Selecciona un vendedor existente o crea uno nuevo, no ambas opciones.");
        }

        if (model.Role != "Admin" && !seleccionoVendedor && !ingresoNuevoVendedor)
        {
            ModelState.AddModelError(nameof(model.VendedorId), "Los roles de ventas deben vincularse a un vendedor.");
        }

        Vendedor? vendedorExistente = null;
        Sucursal? sucursal = null;
        if (seleccionoVendedor)
        {
            vendedorExistente = await db.Vendedores
                .Include(vendedor => vendedor.Sucursal)
                .SingleOrDefaultAsync(vendedor => vendedor.Id == model.VendedorId && vendedor.UsuarioId == null);
            if (vendedorExistente is null)
            {
                ModelState.AddModelError(nameof(model.VendedorId), "El vendedor ya está vinculado o no existe.");
            }
            else
            {
                sucursal = vendedorExistente.Sucursal;
            }
        }

        Vendedor? nuevoVendedor = null;
        if (ingresoNuevoVendedor)
        {
            if (string.IsNullOrWhiteSpace(model.NombreVendedor) || !model.SucursalId.HasValue)
            {
                ModelState.AddModelError(nameof(model.NombreVendedor), "Para crear un vendedor, indica su nombre y sucursal.");
            }
            else
            {
                sucursal = await db.Sucursales.SingleOrDefaultAsync(s => s.Id == model.SucursalId && s.Activo);
                if (sucursal is null)
                {
                    ModelState.AddModelError(nameof(model.SucursalId), "La sucursal seleccionada no existe.");
                }
                else
                {
                    nuevoVendedor = new Vendedor
                    {
                        NombreCompleto = model.NombreVendedor.Trim(),
                        SucursalId = sucursal.Id
                    };
                }
            }
        }

        if (model.Role == "JefeVentas" && sucursal?.JefeVentasId is not null && sucursal.JefeVentasId != vendedorExistente?.Id)
        {
            ModelState.AddModelError(nameof(model.SucursalId), "La sucursal ya tiene un jefe de ventas asignado.");
        }

        if (!ModelState.IsValid)
        {
            await PrepararListasAsync();
            return View(model);
        }

        await using var transaction = await db.Database.BeginTransactionAsync();
        var user = new IdentityUser
        {
            UserName = model.Email.Trim(),
            Email = model.Email.Trim(),
            EmailConfirmed = true,
            LockoutEnabled = true
        };

        var createResult = await userManager.CreateAsync(user, model.Password);
        if (!createResult.Succeeded)
        {
            AgregarErrores(createResult);
            await PrepararListasAsync();
            return View(model);
        }

        var roleResult = await userManager.AddToRoleAsync(user, model.Role);
        if (!roleResult.Succeeded)
        {
            await transaction.RollbackAsync();
            AgregarErrores(roleResult);
            await PrepararListasAsync();
            return View(model);
        }

        Vendedor? vendedorAsignado = vendedorExistente;
        if (vendedorExistente is not null)
        {
            vendedorExistente.UsuarioId = user.Id;
        }
        else if (nuevoVendedor is not null)
        {
            nuevoVendedor.UsuarioId = user.Id;
            db.Vendedores.Add(nuevoVendedor);
            vendedorAsignado = nuevoVendedor;
        }

        await db.SaveChangesAsync();
        if (model.Role == "JefeVentas" && sucursal is not null && vendedorAsignado is not null)
        {
            sucursal.JefeVentasId = vendedorAsignado.Id;
            await db.SaveChangesAsync();
        }

        await transaction.CommitAsync();
        TempData["Mensaje"] = "El usuario se creó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(string id, bool activar)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        if (!activar && user.Id == userManager.GetUserId(User))
        {
            TempData["Error"] = "No puedes desactivar tu propia cuenta.";
            return RedirectToAction(nameof(Index));
        }

        if (!activar && await userManager.IsInRoleAsync(user, "Admin"))
        {
            var administrators = await userManager.GetUsersInRoleAsync("Admin");
            var otherActiveAdministrators = administrators.Count(admin =>
                admin.Id != user.Id && admin.LockoutEnd != DateTimeOffset.MaxValue);
            if (otherActiveAdministrators == 0)
            {
                TempData["Error"] = "No puedes desactivar al último administrador activo.";
                return RedirectToAction(nameof(Index));
            }
        }

        user.LockoutEnabled = true;
        user.LockoutEnd = activar ? null : DateTimeOffset.MaxValue;
        var result = await userManager.UpdateAsync(user);
        TempData[result.Succeeded ? "Mensaje" : "Error"] = result.Succeeded
            ? (activar ? "La cuenta se activó." : "La cuenta se desactivó.")
            : string.Join(" ", result.Errors.Select(error => error.Description));
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestablecerContrasena(string id, string nuevaContrasena)
    {
        if (string.IsNullOrWhiteSpace(nuevaContrasena))
        {
            TempData["Error"] = "Escribe la nueva contraseña.";
            return RedirectToAction(nameof(Index));
        }

        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, nuevaContrasena);
        TempData[result.Succeeded ? "Mensaje" : "Error"] = result.Succeeded
            ? $"Se restableció la contraseña de {user.Email}."
            : string.Join(" ", result.Errors.Select(error => error.Description));
        return RedirectToAction(nameof(Index));
    }

    private async Task PrepararListasAsync()
    {
        ViewBag.Roles = new SelectList(await roleManager.Roles
            .Where(role => role.Name != null && RolesPermitidos.Contains(role.Name))
            .OrderBy(role => role.Name)
            .Select(role => role.Name)
            .ToListAsync());
        ViewBag.Vendedores = new SelectList(await db.Vendedores
            .Include(vendedor => vendedor.Sucursal)
            .Where(vendedor => vendedor.UsuarioId == null && vendedor.Activo)
            .OrderBy(vendedor => vendedor.NombreCompleto)
            .Select(vendedor => new { vendedor.Id, Nombre = vendedor.NombreCompleto + " - " + vendedor.Sucursal.Nombre })
            .ToListAsync(), "Id", "Nombre");
        ViewBag.Sucursales = new SelectList(await db.Sucursales
            .Where(sucursal => sucursal.Activo)
            .OrderBy(sucursal => sucursal.Nombre)
            .Select(sucursal => new { sucursal.Id, sucursal.Nombre })
            .ToListAsync(), "Id", "Nombre");
    }

    private void AgregarErrores(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}