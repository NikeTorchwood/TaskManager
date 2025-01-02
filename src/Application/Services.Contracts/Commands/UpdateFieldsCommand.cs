using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

public record UpdateFieldsCommand(Guid Id,
    string? NewTitle = null,
    string? NewDescription = null,
    DateTime? NewDeadline = null)
    : IRequest<ResultModel<ReadModel>>;