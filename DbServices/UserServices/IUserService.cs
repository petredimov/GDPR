using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.UserServices
{
    public interface IUserService
    {
		List<User> GetByUserID(string userId);
        User Get(string id);
        List<User> GetAll();
        bool Insert(User contract);
        bool Update(User contract);
        bool Delete(string userId);
		string GetRoleIdByName(string roleName);

	}
}