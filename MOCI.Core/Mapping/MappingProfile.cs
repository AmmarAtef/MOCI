using AutoMapper;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using MOCI.Web.Models;

namespace MOCI.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<Terminal, TerminalDto>().ReverseMap();
            CreateMap<NLog, NLogDto>().ReverseMap();
            CreateMap<MappedColumns, MappedColumnsDto>().ReverseMap();
            CreateMap<FINHUB_REVENUE_HEADERPostModel, FINHUB_REVENUE_HEADER>().ReverseMap();
        }
    }
}
