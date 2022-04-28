using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.DAL.Interfaces;
using MOCI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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



        public async Task<List<string>> GetColumnsNames()
        {
            return await _mappedColumnsRepository.GetColumnsNames();
        }

        public async Task<bool> AddColumns(List<MappedColumns> mappedColumns)
        {
            return await _mappedColumnsRepository.AddColumns(mappedColumns);
        }

        public IEnumerable<MappedColumns> GetAll(IEnumerable<MappedColumns> mappedColumns)
        {
            return _mappedColumnsRepository.GetAll();
        }

        public IEnumerable<MappedColumns> GetAllMappedColumns()
        {
            return _mappedColumnsRepository.GetAll();
        }
    }
}
