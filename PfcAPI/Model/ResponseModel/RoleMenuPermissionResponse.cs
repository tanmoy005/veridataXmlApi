namespace PfcAPI.Model.ResponseModel
{
    public class RoleMenuPermissionResponse
    {
        public List<MenuChildren> MenuPermissiondata { get; set; }
    }

    public class MenuChildren
    {
        public string name { get; set; }
        public string icon { get; set; }
        public string route { get; set; }
        public bool active { get; set; }
        public List<MenuChildren> menuchildrens { get; set; }
    }

    //public class RootMenu
    //{
    //    public string name { get; set; }
    //    public string route { get; set; }
    //    public List<MenuChildren> MenuChildren { get; set; }
    //}
}
