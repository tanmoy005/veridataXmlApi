namespace VERIDATA.BLL.Services
{
    public interface IWorkerService
    {
        //public void DoSomething();
        public Task ApiCountMailAsync();

        public Task ApponteeCountMailAsync();

        public Task CriticalAppointeeMail();

        public Task CaseBasedEscalation();
    }
}