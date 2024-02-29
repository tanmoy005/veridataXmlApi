
using PfcAPI.Model.DataAccess;
using PfcAPI.Model.ResponseModel;

namespace PfcAPI.Infrastucture.Interfaces
{
    public interface IRoleMenuDataContext
    {
        public Task<RoleDetails> GetUserRole(int userid);
        public Task<List<MenuNode>> GetMenuData(int userid);
    }
}
