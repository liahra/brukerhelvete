namespace Noested.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public object? ViewModel { get; set; } // To hold the ViewModel that caused the error
        public string? ErrorMessage { get; set; } // To hold a general error message
    }
}