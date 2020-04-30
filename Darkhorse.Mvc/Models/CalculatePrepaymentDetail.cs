namespace DarkHorse.Mvc
{
    public class CalculatePrepaymentDetail
    {
        public int RealPropertyAccountId { get; set; }
        public decimal MonthlyFee { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal TotalDue { get; set; }
        public int Month { get; set; }
    }
}