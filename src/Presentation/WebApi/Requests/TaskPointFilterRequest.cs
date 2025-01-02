using Domain.Entities.Enums;

namespace WebApi.Requests;

public record TaskPointFilterRequest(
    string? SearchTerm,
    DateTime? CreatedAtStartPeriod,
    DateTime? CreatedAtEndPeriod,
    DateTime? StartedAtStartPeriod,
    DateTime? StartedAtEndPeriod,
    DateTime? DeadlineStartPeriod,
    DateTime? DeadlineEndPeriod,
    TaskPointStatuses? TaskPointStatus);