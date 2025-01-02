namespace WebApi.Requests;

public record CreatingTaskPointRequest(
    string Title,
    string Description,
    DateTime Deadline,
    bool IsStarted);