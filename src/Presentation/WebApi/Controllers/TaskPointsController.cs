using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QueryFilterBuilder;
using Services.Contracts.Commands;
using Services.Contracts.Filters;
using Services.Contracts.Models;
using Services.Contracts.Queries;
using WebApi.Requests;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace WebApi.Controllers;

[ApiController, Route("api/v1/[controller]")]
public class TaskPointsController(
    IMediator mediator,
    IMapper mapper) : ControllerBase
{
    [HttpGet("{id:guid}"), ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel<ReadModel>)),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultModel<ReadModel>))]
    public async Task<IActionResult> GetTaskPointById(
        Guid id,
        CancellationToken ct)
    {
        var query = new GetTaskPointByIdQuery(id);
        var result = await mediator.Send(query, ct);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet, ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReadModel>))]
    public async Task<IActionResult> GetAllWithFilters(
        [FromQuery] TaskPointFilterRequest request,
        CancellationToken ct)
    {
        var filters = new List<IFilter<TaskPoint>>();

        if (!string.IsNullOrEmpty(request.SearchTerm))
            filters.Add(new SearchTermFilter(request.SearchTerm));
        if (request.CreatedAtStartPeriod.HasValue || request.CreatedAtEndPeriod.HasValue)
            filters.Add(new CreatedAtPeriodFilter(request.CreatedAtStartPeriod, request.CreatedAtEndPeriod));
        if (request.StartedAtStartPeriod.HasValue || request.StartedAtEndPeriod.HasValue)
            filters.Add(new StartedAtPeriodFilter(request.StartedAtStartPeriod, request.StartedAtEndPeriod));
        if (request.DeadlineStartPeriod.HasValue || request.DeadlineEndPeriod.HasValue)
            filters.Add(new DeadlinePeriodFilter(request.DeadlineStartPeriod, request.DeadlineEndPeriod));
        if (request.TaskPointStatus is not null)
            filters.Add(new StatusFilter(request.TaskPointStatus));

        var queryBuilder = new QueryFilterBuilder<TaskPoint>();

        foreach (var filter in filters)
        {
            queryBuilder.AddFilter(filter.Apply());
        }
        var predicate = queryBuilder.Build();

        var query = new GetAllTaskPointsWithFilterQuery(predicate);

        var taskPoints = await mediator.Send(query, ct);
        return Ok(taskPoints);
    }

    [HttpPost, ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResultModel<ReadModel>)),
     ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultModel<ReadModel>))]
    public async Task<IActionResult> CreateTaskPoint(
        [FromBody] CreatingTaskPointRequest request,
        CancellationToken ct)
    {
        var command = mapper.Map<CreateTaskPointCommand>(request);
        var createdUser = await mediator.Send(command, ct);

        if (!createdUser.Success)
            return BadRequest(createdUser);

        return CreatedAtAction(nameof(GetTaskPointById), new { createdUser.Value.Id }, createdUser);
    }

    [HttpDelete("{id:guid}"), ProducesResponseType(StatusCodes.Status204NoContent),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultModel<bool>))]
    public async Task<IActionResult> MarkAsDeletedTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var command = new MarkAsDeletedCommand(id);
        var result = await mediator.Send(command, ct);

        if (result.Success) return NoContent();
        return NotFound(result);
    }

    [HttpPost("{id:guid}/Cancel"), ProducesResponseType(StatusCodes.Status204NoContent),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultModel<bool>)),
     ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultModel<bool>))]
    public async Task<IActionResult> CancelTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var command = new CancelTaskPointCommand(id);
        var result = await mediator.Send(command, ct);

        if (result.Success) return NoContent();
        if (result.Error == ERROR_MESSAGE_TASK_NOT_FOUND)
            return NotFound(result);
        return BadRequest(result);
    }

    [HttpPost("{id:guid}/Complete"), ProducesResponseType(StatusCodes.Status204NoContent),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultModel<bool>)),
     ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultModel<bool>))]
    public async Task<IActionResult> CompleteTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var command = new CompleteTaskPointCommand(id);
        var result = await mediator.Send(command, ct);

        if (result.Success) return NoContent();
        if (result.Error == ERROR_MESSAGE_TASK_NOT_FOUND)
            return NotFound(result);
        return BadRequest(result);
    }

    [HttpPost("{id:guid}/Start"), ProducesResponseType(StatusCodes.Status204NoContent),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultModel<bool>)),
     ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultModel<bool>))]
    public async Task<IActionResult> StartTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var command = new StartTaskPointCommand(id);
        var result = await mediator.Send(command, ct);

        if (result.Success) return NoContent();
        if (result.Error == ERROR_MESSAGE_TASK_NOT_FOUND)
            return NotFound(result);
        return BadRequest(result);
    }

    [HttpPatch("{id:guid}"), ProducesResponseType(StatusCodes.Status204NoContent),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultModel<bool>)),
     ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultModel<bool>))]
    public async Task<IActionResult> UpdateTaskPointFields(
        Guid id,
        [FromBody] UpdateFieldRequest request,
        CancellationToken ct)
    {
        var command = new UpdateFieldsCommand(
            id,
            request.NewTitle,
            request.NewDescription,
            request.NewDeadline);

        var result = await mediator.Send(command, ct);

        if (result.Success) return NoContent();
        if (result.Error == ERROR_MESSAGE_TASK_NOT_FOUND)
            return NotFound(result);
        return BadRequest(result);
    }
}