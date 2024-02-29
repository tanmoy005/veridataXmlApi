namespace VERIDATA.Model.DataAccess.Response
{
    public class RoleDetailsResponse
    {
        //public int Id { get; set; }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public string? RoleAlias { get; set; }
        public bool? ActiveStatus { get; set; }
    }
}
