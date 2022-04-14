using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOCI.DAL.Interfaces
{
    public interface IUserRepository : IGenericRepository<User, long>
    {
        List<UserListItem> GetList();

        User GetEmployee(string UserName);
    }
}
