using AutoMapper;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.DAL.Interfaces;
using MOCI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MOCI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public List<UserListItem> GetList()
        {
            return _userRepository.GetList();
        }
        public UserDto GetEmployee(string username)
        {
            return _mapper.Map<UserDto>(_userRepository.GetEmployee(username));
        }
        public List<UserDto> GetAll()
        {
            return _mapper.Map<List<UserDto>>(_userRepository.GetAll());
        }

        
        public UserDto GetById(long id)
        {
            return _mapper.Map<UserDto>(_userRepository.GetById(id));
        }

        public void Add(UserDto userDto)
        {
            var entity = _mapper.Map<User>(userDto);
            _userRepository.Add(entity);
        }

        public IList<UserDto> Get(Expression<Func<User, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<UserDto>>(_userRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(UserDto userDto)
        {
            var entity = _mapper.Map<User>(userDto);
            _userRepository.Remove(entity);
        }

        public void Update(UserDto userDto)
        {
            var user = _userRepository.GetById(userDto.Id);
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.IsAdmin = userDto.IsAdmin;
            user.Enabled = userDto.Enabled;
            _userRepository.Update();
        }

        public int Count()
        {
            return _userRepository.Count();
        }

     
    }
}
