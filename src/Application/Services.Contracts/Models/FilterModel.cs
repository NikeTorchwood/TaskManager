using Domain.Entities;
using Services.Contracts.Filters;

namespace Services.Contracts.Models;

public record FilterModel(IEnumerable<IFilter<TaskPoint>> Filters);