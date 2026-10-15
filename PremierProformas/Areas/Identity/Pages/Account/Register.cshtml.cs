using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PremierProformas.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    public IActionResult OnGet() => RedirectToPage("/Account/Login", new { area = "Identity" });

    public IActionResult OnPost() => RedirectToPage("/Account/Login", new { area = "Identity" });
}