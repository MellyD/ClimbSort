using AutoMapper;
using ClimbSort.Core.Models;
using ClimbSort.Core.Models.Generic;
using ClimbSort.Core.Models.Static;
using ClimbSort.Core.ViewModels;
using ClimbSort.Core.ViewModels.Filters;
using ClimbSort.Core.ViewModels.Generic;
using static ClimbSort.Core.Enums;

namespace ClimbSort.Automapper
{
    public class ConfigureAutomapper: Profile
    {
        public ConfigureAutomapper()
        {
            CreateMap<Climb,ClimbModel>()
                .ForMember(dest => dest.ClimbId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CragId, opt => opt.MapFrom(src => src.Crag != null ? src.Crag.Id : (Guid?)null))
                .ForMember(dest => dest.CragName, opt => opt.MapFrom(src => src.Crag != null ? src.Crag.Name : string.Empty))
                .ForMember(dest => dest.GradeLabel, opt => opt.MapFrom(src => src.Grade != null ? src.Grade.GradeLabel : string.Empty))
                .ForMember(dest => dest.GradeScaleOrder, opt => opt.MapFrom(src => src.Grade != null ? src.Grade.ScaleOrder : (int?)null))
                .ForMember(dest => dest.WallTypeId, opt => opt.MapFrom(src => src.WallType.Id))
                .ForMember(dest => dest.TopoImageReference, opt => opt.MapFrom(src => src.Topography != null ? src.Topography.FileReference : null))
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => src.Coordinates))
                .ForMember(dest => dest.TopoCoordinates, opt => opt.MapFrom(src => src.Topography != null ? src.Topography.Coordinates : null))
                .ForMember(dest => dest.CragCoordinates, opt => opt.MapFrom(src => src.Crag != null ? src.Crag.Coordinates : null))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));

            CreateMap<Climb, ClimbSimpleModel>()
                .ForMember(dest => dest.ClimbId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CragName, opt => opt.MapFrom(src => src.Crag != null ? src.Crag.Name : string.Empty))
                .ForMember(dest => dest.WallTypeId, opt => opt.MapFrom(src => src.WallType.Id))
                .ForMember(dest => dest.GradeLabel, opt => opt.MapFrom(src => src.Grade != null ? src.Grade.GradeLabel : string.Empty))
                .ForMember(dest => dest.GradeScaleOrder, opt => opt.MapFrom(src => src.Grade != null ? src.Grade.ScaleOrder : (int?)null))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));

            CreateMap<Coordinates, CoordinatesModel>();

            CreateMap<Crag, CragModel>()
                .ForMember(dest => dest.CragId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => src.Coordinates))
                .ForMember(dest => dest.Climbs, opt => opt.MapFrom(src => src.Climbs))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));

            CreateMap<Crag, CragSimpleModel>()
                .ForMember(dest => dest.CragId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));

            CreateMap<AdvancedClimbFilter, ClimbFilter>()
                .ForMember(dest => dest.MinRating, opt => opt.Ignore())
                .ForMember(dest => dest.MaxRating, opt => opt.Ignore())
                .ForMember(dest => dest.MinGradeScaleOrder, opt => opt.Ignore())
                .ForMember(dest => dest.MaxGradeScaleOrder, opt => opt.Ignore())
                .ForMember(dest => dest.WallTypeIds, opt => opt.Ignore())
                .ForMember(dest => dest.CragId, opt => opt.Ignore())
                .ForMember(dest => dest.MaxPopularity, opt => opt.Ignore())
                .ForMember(dest => dest.MinPopularity, opt => opt.Ignore());

            CreateMap<Tag, TagModel>()
                .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.TagType.Description))
                .ForMember(dest => dest.TagId, opt => opt.MapFrom(src => src.TagType.Id));

            CreateMap<GradingSystem, GradingSystemModel>()
                .ForMember(dest => dest.GradingSystemName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.GradingSystemId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DisciplineId, opt => opt.MapFrom(src => (int)src.Discipline))
                .ForMember(dest => dest.DisciplineName, opt => opt.MapFrom(src => Enum.GetName(src.Discipline)));

            CreateMap<Grade, GradeModel>()
                .ForMember(dest => dest.GradeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.GradingSystemName, opt => opt.MapFrom(src => src.GradingSystem.Name))
                .ForMember(dest => dest.GradingSystemId, opt => opt.MapFrom(src => src.GradingSystem.Id));

            CreateMap<WallType, WallTypeModel>()
                .ForMember(dest => dest.WallTypeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.WallTypeDescription, opt => opt.MapFrom(src => src.Description));
        }
    }
}
