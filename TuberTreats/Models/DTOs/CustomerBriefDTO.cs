namespace TuberTreats.Models.DTOs;

public class CustomerBriefDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public List<TuberOrderBriefDTO> TuberOrders { get; set; }

}