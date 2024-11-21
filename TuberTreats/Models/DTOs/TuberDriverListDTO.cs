using TuberTreats.Models.DTOs;

namespace TuberTreats.Models.DTOs
{
    public class TuberDriverListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TuberDeliveryListDTO> TuberDeliveries { get; set; }
    }
}