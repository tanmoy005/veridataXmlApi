
namespace VERIDATA.Model.DataAccess
{
    public class MenuNodeDetails
    {

        public int MenuId { get; set; }
        public int ParentMenuId { get; set; }
        public string? MenuTitle { get; set; }
        public string? MenuDesc { get; set; }
        public string? DisplayName { get; set; }
        public int MenuLevel { get; set; }
        public string? IconClass { get; set; }
        public string? ActionUrl { get; set; }
        public int? SeqNo { get; set; }
        public string? ActionName { get; set; }
        public string? ActionAlias { get; set; }
        public int? ActionId { get; set; }
    }
}
