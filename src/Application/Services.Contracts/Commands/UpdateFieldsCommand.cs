using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

/// <summary>
/// Command for updating fields of an entity.
/// </summary>
/// <param name="Id">The unique identifier of the entity to be updated.</param>
/// <param name="NewTitle">The new title to be set (optional).</param>
/// <param name="NewDescription">The new description to be set (optional).</param>
/// <param name="NewDeadline">The new deadline to be set (optional).</param>
public record UpdateFieldsCommand(Guid Id,
    string? NewTitle = null,
    string? NewDescription = null,
    DateTime? NewDeadline = null)
    : IRequest<ResultModel<ReadModel>>;