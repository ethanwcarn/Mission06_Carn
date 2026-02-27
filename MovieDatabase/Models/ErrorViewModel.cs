// ErrorViewModel.cs - View model used by the Error view to display error details.
// Passes the request ID to the error page so users can reference it when reporting issues.

namespace MovieDatabase.Models
{
    // Model for the shared Error.cshtml view
    public class ErrorViewModel
    {
        // Unique identifier for the failed request, useful for debugging
        public string? RequestId { get; set; }

        // Returns true when a RequestId exists, controlling whether it's displayed on the error page
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
