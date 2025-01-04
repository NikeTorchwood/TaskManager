using AutoMapper;
using Services.Contracts.Commands;
using WebApi.Requests;

namespace WebApi.Mapping;
public class TaskPointsPresentationProfile : Profile
{
    public TaskPointsPresentationProfile()
    {
        CreateMap<CreatingTaskPointRequest, CreateTaskPointCommand>()
            .ForCtorParam(
                nameof(CreateTaskPointCommand.Title),
                opt => opt.MapFrom(
                    src => src.Title))
            .ForCtorParam(
                nameof(CreateTaskPointCommand.Description),
                opt => opt.MapFrom(
                    src => src.Description))
            .ForCtorParam(
                nameof(CreateTaskPointCommand.Deadline),
                opt => opt.MapFrom(
                    src => src.Deadline))
            .ForCtorParam(
                nameof(CreateTaskPointCommand.IsStarted),
                opt => opt.MapFrom(
                    src => src.IsStarted));
    }
}