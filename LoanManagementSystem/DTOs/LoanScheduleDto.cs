namespace LoanManagementSystem.DTOs
{
    public class LoanScheduleDto
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public decimal PMT { get; set; }
        public DateTime Date { get; set; }
        public LoanScheduleDto(int id, int loanId, decimal pMT, DateTime date)
        {
            Id = id;
            LoanId = loanId;
            PMT = pMT;
            Date = date;
        }
        public LoanScheduleDto() { }
    }
}
