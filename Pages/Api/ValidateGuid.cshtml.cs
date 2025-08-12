using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App1Simulation.Models; // Import the models namespace
using App1Simulation.Services; // Import the new Services namespace for GuidCache
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // For HttpContext.Session (for debugging and other info)
using System; // For Console.WriteLine

namespace App1Simulation.Pages.Api
{
    [IgnoreAntiforgeryToken] // IMPORTANT: Disable Anti-Forgery Token for API endpoints if using POST from another app.
                              // In a real scenario, you'd use HMAC or API keys for cross-app security.
    public class ValidateGuidModel : PageModel
    {
        // OnPostAsync will handle the POST request from App2's backend
        public async Task<IActionResult> OnPostAsync([FromBody] ValidationRequest request)
        {
            // Simulate network delay for App1's processing of the validation request
            await Task.Delay(new Random().Next(50, 200));

            // Retrieve GUID from the shared static cache (the fix!)
            string cachedUserName = null;
            bool guidFoundInCache = GuidCache.TryGetValue(request.GuidToValidate, out cachedUserName);

            // For debugging: Retrieve GUID from current HttpContext.Session (will likely be NULL for server-to-server calls)
            string storedGuidInSession = HttpContext.Session.GetString("App2Guid");
            string storedApp1UserInSession = HttpContext.Session.GetString("App1User");

            // --- DEBUGGING OUTPUT ---
            Console.WriteLine($"--- App1 Validation API Call ---");
            Console.WriteLine($"Received GUID from App2: {request.GuidToValidate ?? "NULL"}");
            Console.WriteLine($"GUID found in static cache: {guidFoundInCache}");
            Console.WriteLine($"Associated User in Cache: {cachedUserName ?? "NULL"}");
            Console.WriteLine($"GUID stored in App1 Session (for reference): {storedGuidInSession ?? "NULL"}");
            Console.WriteLine($"User stored in App1 Session (for reference): {storedApp1UserInSession ?? "NULL"}");
            Console.WriteLine($"Current App1 Request Session ID: {HttpContext.Session.Id}"); // This will be a new session for server-to-server call
            Console.WriteLine($"--- End App1 Validation API Call ---");
            // --- END DEBUGGING OUTPUT ---

            var response = new ValidationResponse();

            if (guidFoundInCache) // Check if the GUID was found in our static cache
            {
                // Simulate successful validation
                response.IsValid = true;
                // In a real scenario, App1 would return the *actual* user's session ID (or a derived one)
                // and real user data associated with the validated GUID.
                response.App1SessionId = HttpContext.Session.Id; // Use App1's API call's session ID for realism
                response.UserName = cachedUserName; // Use the user name retrieved from the cache
                response.Message = "GUID validated successfully by App1 (via shared cache).";

                // IMPORTANT: In a real system, a GUID should typically be single-use.
                // After successful validation, App1 would invalidate or expire this GUID
                // to prevent replay attacks. For simplicity of this simulation, we'll keep it in cache.
                // If you want to simulate single-use GUID: GuidCache.RemoveGuid(request.GuidToValidate);
            }
            else
            {
                // Simulate failed validation
                response.IsValid = false;
                response.App1SessionId = null;
                response.UserName = null;
                response.Message = "Invalid or expired GUID.";
            }

            // Return a JSON response
            return new JsonResult(response);
        }
    }
}
