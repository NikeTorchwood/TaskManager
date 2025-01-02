using AutoMapper;
using Domain.Entities;
using Services.Contracts.Models;

namespace Services.Implementations.Mapping;

public class TaskPointersApplicationProfile : Profile
{
    public TaskPointersApplicationProfile()
    {
        CreateMap<TaskPoint, ReadModel>()
            .ForCtorParam(
                nameof(ReadModel.Id),
                opt => opt.MapFrom(
                    src => src.Id))
            .ForCtorParam(
                nameof(ReadModel.Title),
                opt => opt.MapFrom(
                    src => src.Title.Value))
            .ForCtorParam(
                nameof(ReadModel.Description),
                opt => opt.MapFrom(
                    src => src.Description.Value))
            .ForCtorParam(
                nameof(ReadModel.Deadline),
                opt => opt.MapFrom(
                    src => src.Deadline))
            .ForCtorParam(
                nameof(ReadModel.IsDeleted),
                opt => opt.MapFrom(
                    src => src.IsDeleted))
            .ForCtorParam(
                nameof(ReadModel.StartedAt),
                opt => opt.MapFrom(
                    src => src.StartedAt))
            .ForCtorParam(
                nameof(ReadModel.ClosedAt),
                opt => opt.MapFrom(
                    src => src.ClosedAt))
            .ForCtorParam(
                nameof(ReadModel.Status),
                opt => opt.MapFrom(
                    src => src.Status));
    }
}