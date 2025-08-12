using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http; // For session access

namespace App1Simulation.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Clear all session data to simulate logout
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}
