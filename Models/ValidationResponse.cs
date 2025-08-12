// App1Simulation/Models/ValidationResponse.cs
namespace App1Simulation.Models
{
    public class ValidationResponse
    {
        public bool IsValid { get; set; }
        public string App1SessionId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
