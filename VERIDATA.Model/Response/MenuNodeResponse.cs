namespace VERIDATA.Model.Response
{
    public class MenuNodeResponse
    {
        public bool IsMenu { get; set; }
        public bool IsQuickMenu { get; set; }
        public int Id { get; set; }
        public int Pid { get; set; }
        public int Level { get; set; }
        public string DisplayName { get; set; }
        public string NodeTitle { get; set; }
        public string IconClass { get; set; }
        public string ActionUrl { get; set; }
        public int? SeqNo { get; set; }
        public int? OprnActionId { get; set; }
        public string OprnActionAlias { get; set; }
        public string OprnActionName { get; set; }
        public List<MenuNodeResponse>? Children { get; set; }
        public List<MenuNodeResponse>? OptActions { get; set; }
    }
}
