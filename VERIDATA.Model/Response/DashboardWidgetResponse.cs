namespace VERIDATA.Model.Response
{
    public class DashboardWidgetResponse
    {


        public string? WidgetTypeCode { get; set; }
        public DashboardCardWidgetResponse? WidgetValue { get; set; }

    }
    public class DashboardCardWidgetResponse
    {

        public string? WidgetTypeName { get; set; }
        public string? WidgetTypeCode { get; set; }
        public int? WidgetTypeValue { get; set; }
        public int WidgetFilterDays { get; set; }
        public List<int>? WidgetChartValue { get; set; }
    }
}
