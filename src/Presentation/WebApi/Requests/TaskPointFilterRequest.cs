using Domain.Entities.Enums;

namespace WebApi.Requests;

/// <summary>
/// Represents the request body used to filter task points.
/// </summary>
/// <param name="SearchTerm">Gets or sets the search term to filter task points by title or description.</param>
/// <param name="CreatedAtStartPeriod">Gets or sets the start period for task points creation date filter.</param>
/// <param name="CreatedAtEndPeriod">Gets or sets the end period for task points creation date filter.</param>
/// <param name="StartedAtStartPeriod">Gets or sets the start period for task points start date filter.</param>
/// <param name="StartedAtEndPeriod">Gets or sets the end period for task points start date filter.</param>
/// <param name="DeadlineStartPeriod">Gets or sets the start period for task points deadline filter.</param>
/// <param name="DeadlineEndPeriod">Gets or sets the end period for task points deadline filter.</param>
/// <param name="TaskPointStatus">Gets or sets the status to filter task points by.</param>
public record TaskPointFilterRequest(
    string? SearchTerm,
    DateTime? CreatedAtStartPeriod,
    DateTime? CreatedAtEndPeriod,
    DateTime? StartedAtStartPeriod,
    DateTime? StartedAtEndPeriod,
    DateTime? DeadlineStartPeriod,
    DateTime? DeadlineEndPeriod,
    TaskPointStatuses? TaskPointStatus);