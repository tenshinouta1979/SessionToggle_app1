using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http; // For session access
using System.Threading.Tasks;

namespace App1Simulation.Pages
{
    public class IndexModel : PageModel
    {
        public string LoggedInUser { get; set; }
        public string App2Guid { get; set; }

        public IActionResult OnGet(string guid)
        {
            // Retrieve user info from session (simulated App1 session)
            LoggedInUser = HttpContext.Session.GetString("App1User");
            if (string.IsNullOrEmpty(LoggedInUser))
            {
                // If not logged in, redirect to login page
                return RedirectToPage("/Login");
            }

            // Retrieve the GUID passed in the query string (from Login.cshtml.cs)
            // or directly from session if it was stored there.
            App2Guid = guid ?? HttpContext.Session.GetString("App2Guid");
            
            return Page();
        }
    }
}
