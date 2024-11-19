namespace TuberTreats.Models.DTOs;

public class TuberOrderDTO
{
    public int Id { get; set; }
    public DateTime OrderPlacedOnDate { get; set; }
    public int CustomerId { get; set; }
    public CustomerDTO Customer { get; set; }
    public int TuberDriverId { get; set; }
    public TuberDriverDTO Driver { get; set; }
    public DateTime DeliveredOnDate { get; set; }
    public List<ToppingDTO> Toppings { get; set; }
    public List<string> ToppingNames { get; set; }
    public bool Complete { get; set; }
}