using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MOCI.Services.Interfaces
{
    public interface IUserService
    {
        List<UserListItem> GetList();
        List<UserDto> GetAll();
        UserDto GetById(long id);
        IList<UserDto> Get(Expression<Func<User, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize);
        void Update(UserDto userDto);
        void Remove(UserDto userDto);
        void Add(UserDto userDto);
        int Count();
        UserDto GetEmployee(string username);




    }
}
