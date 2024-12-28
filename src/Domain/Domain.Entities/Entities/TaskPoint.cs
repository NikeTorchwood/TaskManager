using Domain.Entities.Abstractions;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Представляет задачу в системе с точкой выполнения. Хранит информацию о заголовке, описании, статусе,
/// времени создания, дедлайне и времени начала/закрытия.
/// </summary>
public class TaskPoint
    : IEntity<Guid>,
    IDeletableSoftly
{
    /// <summary>
    /// Уникальный идентификатор задачи.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Заголовок задачи.
    /// </summary>
    public Title Title { get; private set; }

    /// <summary>
    /// Описание задачи.
    /// </summary>
    public Description Description { get; private set; }

    /// <summary>
    /// Текущий статус задачи.
    /// </summary>
    public TaskPointStatuses Status { get; private set; }

    /// <summary>
    /// Флаг, показывающий, была ли задача удалена.
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// Дата и время создания задачи.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Дата и время дедлайна задачи.
    /// </summary>
    public DateTime Deadline { get; private set; }

    /// <summary>
    /// Дата и время начала выполнения задачи.
    /// </summary>
    public DateTime? StartedAt { get; private set; }

    /// <summary>
    /// Дата и время завершения задачи.
    /// </summary>
    public DateTime? ClosedAt { get; private set; }

    /// <summary>
    /// Защищённый конструктор для использования в EF.
    /// </summary>
    protected TaskPoint()
    {

    }

    /// <summary>
    /// Инициализирует новый экземпляр задачи.
    /// </summary>
    /// <param name="title">Заголовок задачи.</param>
    /// <param name="description">Описание задачи.</param>
    /// <param name="deadline">Дедлайн задачи.</param>
    /// <param name="isStarted">Флаг, указывающий, была ли задача уже начата. По умолчанию false.</param>
    /// <exception cref="ArgumentNullException">Если заголовок или описание равны null.</exception>
    /// <exception cref="CreatingExpiredDeadlineException">Если дедлайн задачи раньше текущего времени.</exception>
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
    /// Изменяет заголовок задачи. Изменение возможно только для незакрытых задач.
    /// </summary>
    /// <param name="newTitle">Новый заголовок задачи.</param>
    /// <exception cref="ArgumentNullException">Если новый заголовок равен null.</exception>
    /// <exception cref="TaskPointEditingForbiddenException">Если задача уже завершена или отменена.</exception>
    public void ChangeTitle(Title newTitle)
    {
        if (ClosedAt.HasValue)
            throw new TaskPointEditingForbiddenException();
        Title = newTitle
                ?? throw new ArgumentNullException(nameof(newTitle));
    }

    /// <summary>
    /// Изменяет описание задачи. Изменение возможно только для незакрытых задач.
    /// </summary>
    /// <param name="newDescription">Новое описание задачи.</param>
    /// <exception cref="ArgumentNullException">Если новое описание равно null.</exception>
    /// <exception cref="TaskPointEditingForbiddenException">Если задача уже завершена или отменена.</exception>
    public void ChangeDescription(Description newDescription)
    {
        if (ClosedAt.HasValue)
            throw new TaskPointEditingForbiddenException();
        Description = newDescription
                      ?? throw new ArgumentNullException(nameof(newDescription));
    }

    /// <summary>
    /// Изменяет дедлайн задачи. Изменение возможно только для незакрытых задач и если новый дедлайн не истёк.
    /// </summary>
    /// <param name="newDeadline">Новый дедлайн задачи.</param>
    /// <exception cref="CreatingExpiredDeadlineException">Если новый дедлайн раньше текущего времени.</exception>
    /// <exception cref="TaskPointEditingForbiddenException">Если задача уже завершена или отменена.</exception>
    public void ChangeDeadline(DateTime newDeadline)
    {
        if (ClosedAt.HasValue)
            throw new TaskPointEditingForbiddenException();
        if (newDeadline <= DateTime.UtcNow)
            throw new CreatingExpiredDeadlineException(nameof(newDeadline));
        Deadline = newDeadline;
    }

    /// <summary>
    /// Запускает задачу. Статус задачи изменяется на "В процессе".
    /// </summary>
    /// <exception cref="TaskPointAlreadyClosedException">Если задача уже завершена или отменена.</exception>
    public void StartTaskPoint()
    {
        if (ClosedAt.HasValue)
            throw new TaskPointAlreadyClosedException();

        if (StartedAt.HasValue) return;
        StartedAt = DateTime.UtcNow;
        Status = TaskPointStatuses.InProgress;
    }

    /// <summary>
    /// Отменяет задачу. Статус задачи изменяется на "Отменено", и задача считается закрытой.
    /// </summary>
    /// <exception cref="TaskPointAlreadyClosedException">Если задача уже завершена или отменена.</exception>
    public void CancelTaskPoint()
    {
        if (ClosedAt.HasValue)
            throw new TaskPointAlreadyClosedException();

        Status = TaskPointStatuses.Cancelled;
        ClosedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Завершает задачу. Статус задачи изменяется на "Завершено", и задача считается закрытой.
    /// </summary>
    /// <exception cref="TaskPointNotStartedException">Если задача не была начата.</exception>
    /// <exception cref="TaskPointAlreadyClosedException">Если задача уже завершена или отменена.</exception>
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
    /// Помечает задачу как удалённую (мягкое удаление). Не влияет на статус задачи.
    /// </summary>
    public void MarkAsDeleted() => IsDeleted = true;
}