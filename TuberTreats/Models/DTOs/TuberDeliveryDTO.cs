namespace TuberTreats.Models.DTOs;

public class TuberDeliveryDTO
{
    public int Id { get; set; }
    public DateTime OrderPlacedOnDate { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public DateTime DeliveredOnDate { get; set; }
    public List<string> Toppings { get; set; }
}