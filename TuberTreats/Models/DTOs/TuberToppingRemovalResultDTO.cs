namespace TuberTreats.Models.DTOs;

public class TuberToppingRemovalResultDTO
{
    public int OrderId { get; set; }
    public int RemovedToppingId { get; set; }
    public List<ToppingDTO> RemainingToppings { get; set; }
}