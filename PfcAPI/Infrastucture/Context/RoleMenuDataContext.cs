using PfcAPI.Infrastucture.DBContext;
using PfcAPI.Infrastucture.Interfaces;
using PfcAPI.Infrastucture.utility;
using PfcAPI.Model.DataAccess;
using PfcAPI.Model.ResponseModel;

namespace PfcAPI.Infrastucture.Context
{
    public class RoleMenuDataContext : IRoleMenuDataContext
    {
        private readonly DbContextDB _context;
        public RoleMenuDataContext(DbContextDB context)
        {
            _context = context;
        }
        public Task<RoleDetails> GetUserRole(int userid)
        {
            try
            {
                var userRole = from p in _context.RoleUserMapping
                               join a in _context.RoleMaster
                                   on p.RoleId equals a.RoleId
                               where p.ActiveStatus == true & a.ActiveStatus == true & p.UserId == userid
                               select new { p, a.RoleName, a.RoleDesc };

                var userRoleData = userRole.Select(x => new RoleDetails
                {
                    RoleName = x.RoleName,
                    RoleId = x.p.RoleId,
                    RoleDescription = x.RoleDesc,
                }).FirstOrDefault();


                return Task.FromResult(userRoleData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<MenuNode>> GetMenuData(int userid)
        {
            var data = await GetMenuLeafNodeList(userid);
            var _data = data.DistinctBy(x => x.Id).OrderBy(x => x.Id).ToList();
            var mainMenuLst = _data.Where(x => x.Pid == 0 && x.Level == 1)?.ToList();
            //_data?.ForEach(i =>
            //{
            //    var _OptActions = data.Where(ch => (ch.Id == i.Id) && (ch.OprnActionId > 0)).ToList();
            //    i.OptActions = _OptActions.DistinctBy(x => x.OprnActionId).ToList();
            //});

            //mainMenuLst?.ForEach(
            //    x => x.Children = findChild(_data, x.Level + 1, x.Id)

            //);

            return mainMenuLst;
        }
        private async Task<List<MenuNode>> GetMenuLeafNodeList(int userid)
        {
            var userRole = await GetUserRole(userid);

            var _menudata = from r in _context.MenuRoleMapping
                            join m in _context.MenuMaster
                                on r.MenuId equals m.MenuId
                            where r.RoleId == userRole.RoleId & m.ActiveStatus == true & r.ActiveStatus == true
                            orderby m.SeqNo ascending
                            select new
                            {
                                m.MenuId,
                                m.ParentMenuId,
                                m.MenuTitle,
                                m.MenuDesc,
                                m.menu_level,
                                m.menu_icon_url,
                                m.menu_action,
                                m.SeqNo,
                                r.ActionId
                            };
            var menudata = _menudata
                    .Join(_context.ActionMaster.Where(x => x.ActiveStatus == true),
                        a => a.ActionId,
                        m => m.ActionId,
                        (m, a) => new
                        {
                            m.MenuId,
                            m.ParentMenuId,
                            m.MenuTitle,
                            m.MenuDesc,
                            m.menu_level,
                            m.menu_icon_url,
                            m.menu_action,
                            m.SeqNo,
                            a.ActionName,
                            a.ActionAlias,
                            a.ActionId
                            //  r.ActionId
                        }).ToList();


            var data = new List<MenuNode>();

            menudata?.ToList().ForEach(x =>
            {
                var _OptActions = menudata.Where(ch => (ch.MenuId == x.MenuId) && (ch.ActionId > 0)).DistinctBy(x => x.ActionId).ToList();
                var _curActions = _OptActions?.Select((xa) => new MenuNode
                {
                    OprnActionId = xa?.ActionId ?? 0,
                    OprnActionAlias = xa?.ActionAlias ?? string.Empty,
                    OprnActionName = xa?.ActionName ?? string.Empty
                })?.ToList();
                var _isQuickMenu = x.menu_level == 1 && string.IsNullOrEmpty(x.menu_action) ? true : false;
                data.Add(new MenuNode()
                {
                    IsMenu = x.menu_level == 1 ? true : false,
                    IsQuickMenu = _isQuickMenu,
                    Id = x.MenuId,
                    Pid = x.ParentMenuId,
                    Level = x.menu_level,
                    ActionUrl = x.menu_action,
                    DisplayName = x.MenuTitle,
                    IconClass = x.menu_icon_url,
                    NodeTitle = x.MenuDesc,
                    SeqNo = x.SeqNo,
                    OprnActionId = x?.ActionId ?? 0,
                    OprnActionAlias = x?.ActionAlias ?? string.Empty,
                    OprnActionName = x?.ActionName ?? string.Empty,
                    OptActions = !_isQuickMenu ? _curActions : null,
                    Children = null,
                });
            });

            data.ForEach(i =>
            {
                i.Children = data.Where(ch => ch.Pid == i.Id).DistinctBy(x => x.Id).ToList();
            });
            //data = data.Where(x => x.Level == 1).ToList();
            return data;
        }
        private List<MenuNode> GenerateNestedMenuNodes(List<MenuNode> menus)
        {
            var uniqueMenus = menus.Where(x => x.IsMenu).DistinctBy(x => x.Id)?.ToList();
            var result = new List<MenuNode>();

            uniqueMenus.ForEachWithIndex((x, i) =>
            {
                // var menuItem = GenerateMenuNode(x, url);
                if (x.Children != null && x.Children.Any(y => y.IsMenu))
                    x.Children = GenerateNestedMenuNodes(x.Children);

                result.Add(x);
            });

            return result;
        }
        private List<MenuNode> findChild(List<MenuNode> lst, int chldlevel, int pid)
        {
            var chldLst = new List<MenuNode>();
            lst?.ForEach(x =>
            {
                if (x.Pid == pid && x.Level == chldlevel)
                {
                    x.Children = findChild(lst, x.Level + 1, x.Id);
                    chldLst.Add(x);

                }
            });
            chldLst = chldLst.Count() > 0 ? chldLst : null;
            return chldLst;
        }

    }
}
