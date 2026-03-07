namespace AccountsAppWeb.Core.Models
{
    public class ScheduleWiseReportModel
    {
        public int Id { get; set; }
        public string InstId { get; set; }
        public string AccountGroupName { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
    }
}
