using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace aspnetmvc_starter.Web.Utility
{
    public static class MapperExtentions
    {
        private static IMapper _mapper;

        static MapperExtentions()
        {
            _mapper = AutoMapperProfile.Init();
        }
        
        // Generic Implementation
        public static TDestination MapTo<TDestination>(this object source)
        {
            return _mapper.Map<TDestination>(source);
        }
        
        public static IQueryable<TDestination> MapTo<TDestination>(this IQueryable source)
        {
            return source.ProjectTo<TDestination>(_mapper.ConfigurationProvider);
        }
        
        public static List<TDestination> MapTo<TDestination>(this List<object> entity)
        {
            return _mapper.Map<List<TDestination>>(entity);
        }
        
    }
}