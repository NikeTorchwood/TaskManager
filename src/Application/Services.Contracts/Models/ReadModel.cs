using Domain.Entities.Enums;

namespace Services.Contracts.Models;

public record ReadModel(
    Guid Id,
    string Title,
    string Description,
    TaskPointStatuses Status,
    bool IsDeleted,
    DateTime CreatedAt,
    DateTime Deadline,
    DateTime? StartedAt,
    DateTime? ClosedAt);