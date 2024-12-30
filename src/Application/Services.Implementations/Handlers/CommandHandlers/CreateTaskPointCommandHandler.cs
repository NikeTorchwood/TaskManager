using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;

namespace Services.Implementations.Handlers.CommandHandlers;

internal class CreateTaskPointCommandHandler(
    IWriteTaskPointsRepository repository,
    IMapper mapper)
    : IRequestHandler<CreateTaskPointCommand, ReadModel>
{
    public async Task<ReadModel> Handle(
        CreateTaskPointCommand request,
        CancellationToken ct)
    {
        var title = new Title(request.Title);
        var description = new Description(request.Description);

        var taskPoint = new TaskPoint(title, description, request.Deadline, request.IsStarted);

        var createdTaskPoint = await repository.AddAsync(taskPoint, ct);

        return mapper.Map<ReadModel>(createdTaskPoint);
    }
}