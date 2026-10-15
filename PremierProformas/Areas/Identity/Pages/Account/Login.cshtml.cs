using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PremierProformas.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel(SignInManager<IdentityUser> signInManager) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await signInManager.PasswordSignInAsync(
            Input.Email,
            Input.Password,
            Input.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return LocalRedirect(Url.IsLocalUrl(ReturnUrl) ? ReturnUrl! : Url.Content("~/"));
        }

        ModelState.AddModelError(string.Empty, result.IsLockedOut
            ? "La cuenta está bloqueada temporalmente. Intenta de nuevo en 15 minutos."
            : "Correo o contraseña incorrectos.");
        return Page();
    }

    public class InputModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Escribe un correo válido.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }
}