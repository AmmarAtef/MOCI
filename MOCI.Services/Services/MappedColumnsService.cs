using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.DAL.Interfaces;
using MOCI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.Services.Services
{
    public class MappedColumnsService : IMappedColumnsService
    {
        private readonly IMappedColumnsRepository _mappedColumnsRepository;
        private readonly IMapper _mapper;

        public MappedColumnsService(IMappedColumnsRepository mappedColumnsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _mappedColumnsRepository = mappedColumnsRepository;
        }

        

        public List<string> GetColumnsNames()
        {
            return _mappedColumnsRepository.GetColumnsNames();
        }

    }
}
