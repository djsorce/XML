using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<bool?> AddUserToXML(UserDetail User);
        Task<List<UserDetail>?> GetListOfUsers();
        Task<bool> UpdateUserDetail(UserDetail User, string PreviousCellphone);
        Task<UserDetail> GetSingleUserByCell(string Cellphone);
        Task<bool?> DeleteUserByCell(string Cellphone);
    }
}
