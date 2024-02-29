namespace PfcAPI.Services
{
    public interface IWorkerService
    {
        //public void DoSomething();
        public Task apiCountMailAsync();
        public Task ApponteeCountMailAsync();
        public Task criticalAppointeeMail();
        public Task CaseBasedEscalation();
    }
}
