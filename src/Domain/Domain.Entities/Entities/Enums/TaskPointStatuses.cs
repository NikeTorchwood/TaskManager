namespace Domain.Entities.Enums;

/// <summary>
/// Task point statuses in the task management system.
/// </summary>
public enum TaskPointStatuses
{
    /// <summary>
    /// The task has been created, but not started yet.
    /// </summary>
    Created = 0,

    /// <summary>
    /// The task is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The task was completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// The task has been cancelled.
    /// </summary>
    Cancelled = 3
}