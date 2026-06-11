using AutoMapper;
using FontRecommender.Core.Models;
using FontRecommender.Core.Models.Generic;
using FontRecommender.Core.ViewModels;
using FontRecommender.Core.ViewModels.Filters;
using FontRecommender.Core.ViewModels.Generic;

namespace FontRecommender.Automapper
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
                .ForMember(dest => dest.WallTypeId, opt => opt.MapFrom(src => src.WallType.Id))
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => src.Coordinates));

            CreateMap<Climb, ClimbSimpleModel>()
                .ForMember(dest => dest.ClimbId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CragName, opt => opt.MapFrom(src => src.Crag != null ? src.Crag.Name : string.Empty))
                .ForMember(dest => dest.WallTypeId, opt => opt.MapFrom(src => src.WallType.Id))
                .ForMember(dest => dest.GradeLabel, opt => opt.MapFrom(src => src.Grade != null ? src.Grade.GradeLabel : string.Empty));

            CreateMap<Coordinates, CoordinatesModel>();

            CreateMap<Crag, CragModel>()
                .ForMember(dest => dest.CragId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => src.Coordinates));

            CreateMap<Crag, CragSimpleModel>()
                .ForMember(dest => dest.CragId, opt => opt.MapFrom(src => src.Id));

            CreateMap<AdvancedClimbFilter, ClimbFilter>()
                .ForMember(dest => dest.MinRating, opt => opt.Ignore())
                .ForMember(dest => dest.MaxRating, opt => opt.Ignore())
                .ForMember(dest => dest.MinGradeScaleOrder, opt => opt.Ignore())
                .ForMember(dest => dest.MaxGradeScaleOrder, opt => opt.Ignore())
                .ForMember(dest => dest.WallTypeIds, opt => opt.Ignore())
                .ForMember(dest => dest.CragId, opt => opt.Ignore())
                .ForMember(dest => dest.MaxPopularity, opt => opt.Ignore())
                .ForMember(dest => dest.MinPopularity, opt => opt.Ignore());
        }
    }
}
