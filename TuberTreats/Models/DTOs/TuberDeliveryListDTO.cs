namespace TuberTreats.Models.DTOs
{
    public class TuberDeliveryListDTO
    {
        public int Id { get; set; }
        public DateTime OrderPlacedOnDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime DeliveredOnDate { get; set; }
    }
}