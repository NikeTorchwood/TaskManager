using Domain.Entities;
using Services.Contracts.Filters;

namespace Services.Contracts.Models;

/// <summary>
/// Represents a model containing a collection of filters to be applied to <see cref="TaskPoint"/> entities.
/// </summary>
public record FilterModel(IEnumerable<IFilter<TaskPoint>> Filters);