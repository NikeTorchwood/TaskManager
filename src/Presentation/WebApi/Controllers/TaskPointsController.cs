using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Filters;
using Services.Contracts.Models;
using WebApi.Requests;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TaskPointsController(
    ITaskPointsService service,
    IMapper mapper) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTaskPointById(
        Guid id,
        CancellationToken ct)
    {
        var result = await service.GetTaskPointByIdAsync(id, ct);

        return HandleResultModel(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWithFilter(
        [FromQuery] TaskPointFilterRequest request,
        CancellationToken ct)
    {
        var filters = new List<IFilter<TaskPoint>>();

        if (!string.IsNullOrEmpty(request.SearchTerm))
            filters.Add(new SearchTermFilter(request.SearchTerm));
        if (request.CreatedAtStartPeriod.HasValue || request.CreatedAtEndPeriod.HasValue)
            filters.Add(new CreatedAtPeriodFilter(request.CreatedAtStartPeriod, request.CreatedAtEndPeriod));
        if(request.StartedAtStartPeriod.HasValue || request.StartedAtEndPeriod.HasValue)
            filters.Add(new StartedAtPeriodFilter(request.StartedAtStartPeriod, request.StartedAtEndPeriod));
        if (request.DeadlineStartPeriod.HasValue || request.DeadlineEndPeriod.HasValue)
            filters.Add(new DeadlinePeriodFilter(request.DeadlineStartPeriod, request.DeadlineEndPeriod));
        if (request.TaskPointStatus is not null)
            filters.Add(new StatusFilter(request.TaskPointStatus));

        var filterModel = new FilterModel(filters);
        var taskPoints = await service.GetAllTaskPointsWithFilterAsync(filterModel, ct);
        return Ok(taskPoints);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTaskPoint(
        [FromBody] CreatingTaskPointRequest request,
        CancellationToken ct)
    {
        var command = mapper.Map<CreateTaskPointCommand>(request);
        var createdUser = await service.CreateTaskPointAsync(command, ct);

        if (!createdUser.Success)
            return BadRequest(createdUser);

        return CreatedAtAction(nameof(GetTaskPointById), new { createdUser.Value.Id }, createdUser.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> MarkAsDeletedTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var result = await service.MarkAsDeletedTaskPointAsync(id, ct);

        return HandleResultModel(result);
    }

    [HttpPost("{id:guid}/Cancel")]
    public async Task<IActionResult> CancelTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var result = await service.CancelTaskPointAsync(id, ct);

        return HandleResultModel(result);
    }

    [HttpPost("{id:guid}/Complete")]
    public async Task<IActionResult> CompleteTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var result = await service.CompleteTaskPointAsync(id, ct);

        return HandleResultModel(result);
    }

    [HttpPost("{id:guid}/Start")]
    public async Task<IActionResult> StartTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var result = await service.StartTaskPointAsync(id, ct);

        return HandleResultModel(result);
    }

    [HttpPatch("{id:guid}")]
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

        var result = await service.UpdateTaskPointFieldsAsync(command, ct);
        return HandleResultModel(result);
    }

    private IActionResult HandleResultModel<T>(ResultModel<T> result)
    {
        if (result.Success)
        {
            if (typeof(T) == typeof(bool) || result.Value is null)
                return NoContent();

            return Ok(result.Value);
        }

        if (result.Error == ERROR_MESSAGE_TASK_NOT_FOUND)
            return NotFound(new { Message = result.Error });

        return BadRequest(result.Error);
    }
}