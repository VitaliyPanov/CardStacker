using CardStacker.Data;
using CardStacker.General.Services;

namespace CardStacker.General.Controllers
{
    public interface IControllersMediator
    {
        public void Initialize(ICoreController coreController, IUIController uiController, IDataService dataService, RuntimeData runtimeData);
        void IncreaseScore();
        void IncreaseLevel();
        void QuitGame();
        void GameOver();
        void RestartGame();
    }
}