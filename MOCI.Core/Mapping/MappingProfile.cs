using AutoMapper;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<Terminal, TerminalDto>().ReverseMap();
            CreateMap<NLog, NLogDto>().ReverseMap();

        
        }
    }
}
