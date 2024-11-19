namespace TuberTreats.Models
{
    public class TuberDelivery
    {
        public int Id { get; set; }
        public int TuberOrderId { get; set; }
        public int TuberDriverId { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}