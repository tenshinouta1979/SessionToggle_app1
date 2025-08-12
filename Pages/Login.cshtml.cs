using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http; // For session management
using System;
using System.Threading.Tasks;
using App1Simulation.Services; // Import the new Services namespace

namespace App1Simulation.Pages
{
    public class LoginModel : PageModel
    {
        // Bind properties for Username and Password from the form
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        // Property to display error messages on the page
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Clear any existing error messages on page load
            ErrorMessage = "";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Hardcoded credentials for demonstration
            const string ValidUsername = "user";
            const string ValidPassword = "password123";

            if (Username == ValidUsername && Password == ValidPassword)
            {
                // Simulate successful authentication and App1 session creation
                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetString("App1User", Username);

                // 1. Generate a GUID for App2
                string guid = Guid.NewGuid().ToString();

                // 2. Store this GUID in the shared static cache so App1's API can validate it.
                // This is the key fix for the "GUID stored in App1 Session: NULL" problem.
                GuidCache.AddGuid(guid, Username);

                // 3. (Optional) Still store in HttpContext.Session for potential other uses
                // e.g., if App1 itself needed to retrieve the GUID later via its own session.
                HttpContext.Session.SetString("App2Guid", guid);

                // Redirect to a dashboard or a page that will embed App2, passing the GUID.
                return RedirectToPage("/Index", new { guid = guid });
            }
            else
            {
                // Authentication failed
                ErrorMessage = "Invalid username or password. Please try again.";
                return Page(); // Stay on the login page and display the error
            }
        }
    }
}
