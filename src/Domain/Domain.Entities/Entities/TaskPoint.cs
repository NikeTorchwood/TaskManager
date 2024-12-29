using Domain.Entities.Abstractions;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Represents a task in the system with a point of completion. Stores information about the title, description, status,
/// creation time, deadline, and start time/closures.
/// </summary>
public class TaskPoint
    : IEntity<Guid>,
    IDeletableSoftly
{
    /// <summary>
    /// The unique identifier of the issue.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Task title.
    /// </summary>
    public Title Title { get; private set; }

    /// <summary>
    /// Task description.
    /// </summary>
    public Description Description { get; private set; }

    /// <summary>
    /// The current status of the issue.
    /// </summary>
    public TaskPointStatuses Status { get; private set; }

    /// <summary>
    /// A flag indicating whether the task has been deleted.
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// Date and time when the task was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Date and time of the task deadline.
    /// </summary>
    public DateTime Deadline { get; private set; }

    /// <summary>
    /// Date and time of the start of the task.
    /// </summary>
    public DateTime? StartedAt { get; private set; }

    /// <summary>
    /// Date and time of completion of the task.
    /// </summary>
    public DateTime? ClosedAt { get; private set; }

    /// <summary>
    /// A secure constructor for use in EF.
    /// </summary>
    protected TaskPoint()
    {

    }

    /// <summary>
    /// Initializes a new instance of the task.
    /// </summary>
    /// <param name="title">The task title.</param>
    /// <param name="description">Task description.</param>
    /// <param name="deadline">The deadline for the task.</param>
    /// <param name="isStarted">A flag indicating whether the task has already been started. By default, false.</param>
    /// <exception cref="ArgumentNullException">Thrown if the title or description is null.</exception>
    /// <exception cref="CreatingExpiredDeadlineException">Thrown if the task deadline is earlier than the current time.</exception>
    public TaskPoint(Title title, Description description, DateTime deadline, bool isStarted = false)
    {
        Id = Guid.NewGuid();
        Title = title
                ?? throw new ArgumentNullException(nameof(title));
        Description = description
                      ?? throw new ArgumentNullException(nameof(description));

        Status = TaskPointStatuses.Created;
        CreatedAt = DateTime.UtcNow;

        if (deadline <= DateTime.UtcNow)
            throw new CreatingExpiredDeadlineException(nameof(deadline));
        Deadline = deadline;

        if (!isStarted) return;
        StartedAt = DateTime.UtcNow;
        Status = TaskPointStatuses.InProgress;
    }

    /// <summary>
    /// Changes the task title. The change is only possible for unclosed tasks.
    /// </summary>
    /// <param name="newTitle">New task title.</param>
    /// <exception cref="ArgumentNullException">Thrown if the new title is null.</exception>
    /// <exception cref="TaskPointEditingForbiddenException">Thrown if the task has already been completed or cancelled.</exception>
    public void ChangeTitle(Title newTitle)
    {
        if (ClosedAt.HasValue)
            throw new TaskPointEditingForbiddenException();
        Title = newTitle
                ?? throw new ArgumentNullException(nameof(newTitle));
    }

    /// <summary>
    /// Modifies the task description. The change is only possible for unclosed tasks.
    /// </summary>
    /// <param name="newDescription">New task description.</param>
    /// <exception cref="ArgumentNullException">Thrown if the new description is null.</exception>
    /// <exception cref="TaskPointEditingForbiddenException">Thrown if the task has already been completed or cancelled.</exception>
    public void ChangeDescription(Description newDescription)
    {
        if (ClosedAt.HasValue)
            throw new TaskPointEditingForbiddenException();
        Description = newDescription
                      ?? throw new ArgumentNullException(nameof(newDescription));
    }

    /// <summary>
    /// Changes the task deadline. The change is only possible for unclosed tasks and if the new deadline has not expired.
    /// </summary>
    /// <param name="newDeadline">New task deadline.</param>
    /// <exception cref="CreatingExpiredDeadlineException">Thrown if the new deadline is earlier than the current time.</exception>
    /// <exception cref="TaskPointEditingForbiddenException">Thrown if the task has already been completed or cancelled.</exception>
    public void ChangeDeadline(DateTime newDeadline)
    {
        if (ClosedAt.HasValue)
            throw new TaskPointEditingForbiddenException();
        if (newDeadline <= DateTime.UtcNow)
            throw new CreatingExpiredDeadlineException(nameof(newDeadline));
        Deadline = newDeadline;
    }

    /// <summary>
    /// Starts the task. The task status changes to "In progress".
    /// </summary>
    /// <exception cref="TaskPointAlreadyClosedException">Thrown if the task has already been completed or cancelled.</exception>
    public void StartTaskPoint()
    {
        if (ClosedAt.HasValue)
            throw new TaskPointAlreadyClosedException();

        if (StartedAt.HasValue) return;
        StartedAt = DateTime.UtcNow;
        Status = TaskPointStatuses.InProgress;
    }

    /// <summary>
    /// Cancels the task. The task status changes to "Canceled" and the task is considered closed.
    /// </summary>
    /// <exception cref="TaskPointAlreadyClosedException">Thrown if the task has already been completed or cancelled.</exception>
    public void CancelTaskPoint()
    {
        if (ClosedAt.HasValue)
            throw new TaskPointAlreadyClosedException();

        Status = TaskPointStatuses.Cancelled;
        ClosedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Completes the task. The task status changes to "Completed" and the task is considered closed.
    /// </summary>
    /// <exception cref="TaskPointNotStartedException">Thrown if the task has not been started.</exception>
    /// <exception cref="TaskPointAlreadyClosedException">Thrown if the task has already been completed or cancelled.</exception>
    public void CompleteTaskPoint()
    {
        if (!StartedAt.HasValue)
            throw new TaskPointNotStartedException();
        if (ClosedAt.HasValue)
            throw new TaskPointAlreadyClosedException();

        Status = TaskPointStatuses.Completed;
        ClosedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the task as deleted (soft deletion). It does not affect the issue status.
    /// </summary>
    public void MarkAsDeleted() => IsDeleted = true;
}