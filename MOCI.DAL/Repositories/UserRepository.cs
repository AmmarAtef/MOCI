using Microsoft.EntityFrameworkCore;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.Core.Enums;
using MOCI.DAL.DbContexts;
using MOCI.DAL.Interfaces;
using MOCI.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOCI.DAL.Interfaces
{
    public class UserRepository : GenericRepository<User, long>, IUserRepository
    {
        public UserRepository(MTRSDBContext context) : base(context)
        {
        }

        public List<UserListItem> GetList()
        {
            var query = from user in _context.Users
                        select new UserListItem() {
                            Name = user.FirstName + " " + user.LastName,
                          
                            Id = user.Id
                        };

            return query.ToList();
        }


        public User GetEmployee(string UserName)
        {


            var user = _context.Users.Where(x => x.UserName == UserName).FirstOrDefault();


            return user;
        }




    }
}