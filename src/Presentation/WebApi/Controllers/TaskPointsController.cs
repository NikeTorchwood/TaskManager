using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Filters;
using Services.Contracts.Models;
using WebApi.Requests;
using WebApi.Responses;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace WebApi.Controllers;

[ApiController, Route("api/v1/[controller]")]
public class TaskPointsController(
    ITaskPointsService service,
    IMapper mapper) : ControllerBase
{
    [HttpGet("{id:guid}"), ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ReadModel>)),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))]
    public async Task<IActionResult> GetTaskPointById(
        Guid id,
        CancellationToken ct)
    {
        var result = await service.GetTaskPointByIdAsync(id, ct);

        if (!result.Success)
            return NotFound(new ApiResponse<string>(result.Error));

        return Ok(new ApiResponse<ReadModel>(result.Value));
    }

    [HttpGet, ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ReadModel>))]
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

        var filterModel = new FilterModel(filters);
        var taskPoints = await service.GetAllTaskPointsWithFilterAsync(filterModel, ct);
        return Ok(new ApiResponse<IEnumerable<ReadModel>>(taskPoints));
    }

    [HttpPost, ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<ReadModel>)),
     ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))]
    public async Task<IActionResult> CreateTaskPoint(
        [FromBody] CreatingTaskPointRequest request,
        CancellationToken ct)
    {
        var command = mapper.Map<CreateTaskPointCommand>(request);
        var createdUser = await service.CreateTaskPointAsync(command, ct);

        if (!createdUser.Success)
            return BadRequest(new ApiResponse<string>(createdUser.Error));

        var result = new ApiResponse<ReadModel>(createdUser.Value);

        return CreatedAtAction(nameof(GetTaskPointById), new { result.Data.Id }, result);
    }

    [HttpDelete("{id:guid}"), ProducesResponseType(StatusCodes.Status204NoContent),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))]
    public async Task<IActionResult> MarkAsDeletedTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var result = await service.MarkAsDeletedTaskPointAsync(id, ct);

        if (result.Success) return NoContent();
        return NotFound(new ApiResponse<string>(result.Error));
    }

    [HttpPost("{id:guid}/Cancel"), ProducesResponseType(StatusCodes.Status204NoContent),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>)),
     ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))]
    public async Task<IActionResult> CancelTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var result = await service.CancelTaskPointAsync(id, ct);

        if (result.Success) return NoContent();
        if (result.Error == ERROR_MESSAGE_TASK_NOT_FOUND)
            return NotFound(new ApiResponse<string>(result.Error));
        return BadRequest(new ApiResponse<string>(result.Error));
    }

    [HttpPost("{id:guid}/Complete"), ProducesResponseType(StatusCodes.Status204NoContent),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>)),
     ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))]
    public async Task<IActionResult> CompleteTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var result = await service.CompleteTaskPointAsync(id, ct);

        if (result.Success) return NoContent();
        if (result.Error == ERROR_MESSAGE_TASK_NOT_FOUND)
            return NotFound(new ApiResponse<string>(result.Error));
        return BadRequest(new ApiResponse<string>(result.Error));
    }

    [HttpPost("{id:guid}/Start"), ProducesResponseType(StatusCodes.Status204NoContent),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>)),
     ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))]
    public async Task<IActionResult> StartTaskPoint(
        Guid id,
        CancellationToken ct)
    {
        var result = await service.StartTaskPointAsync(id, ct);

        if (result.Success) return NoContent();
        if (result.Error == ERROR_MESSAGE_TASK_NOT_FOUND)
            return NotFound(new ApiResponse<string>(result.Error));
        return BadRequest(new ApiResponse<string>(result.Error));
    }

    [HttpPatch("{id:guid}"), ProducesResponseType(StatusCodes.Status204NoContent),
     ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>)),
     ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))]
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

        if (result.Success) return NoContent();
        if (result.Error == ERROR_MESSAGE_TASK_NOT_FOUND)
            return NotFound(new ApiResponse<string>(result.Error));
        return BadRequest(new ApiResponse<string>(result.Error));
    }
}