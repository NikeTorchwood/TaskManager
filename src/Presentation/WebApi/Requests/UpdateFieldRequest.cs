namespace WebApi.Requests;

public class UpdateFieldRequest
{
    public DateTime? NewDeadline { get; set; } = null;
    public string? NewTitle { get; set; } = null;
    public string? NewDescription { get; set; } = null;
}