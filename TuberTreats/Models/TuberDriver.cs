namespace TuberTreats.Models;

public class TuberDriver
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TuberDelivery> TuberDeliveries { get; set; }
}
