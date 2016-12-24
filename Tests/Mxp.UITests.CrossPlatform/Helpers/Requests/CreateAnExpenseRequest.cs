namespace Mxp.UITests.CrossPlatform.Helpers.Requests
{
    public class CreateAnExpenseRequest
    {
        public string Category { get; set; }
        public string Country { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public bool AddReceipt { get; set; }
        public bool IsCommentMandatory { get; set; }
    }
}
