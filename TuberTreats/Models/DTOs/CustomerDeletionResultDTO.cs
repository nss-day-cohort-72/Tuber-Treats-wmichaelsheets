namespace TuberTreats.Models.DTOs;

public class CustomerDeletionResultDTO
{
    public int DeletedCustomerId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}