namespace CardStacker.General.Controllers
{
    public interface ILogicController : IController
    {
        void Initialize(IControllersMediator mediator);
    }
}