using CardStacker.Data;
using CardStacker.General.Controllers;
using CardStacker.General.Services;
using UnityEngine;

namespace CardStacker.Core.GameControllers
{
    internal sealed class ControllersMediator : MonoBehaviour, IControllersMediator
    {
        private IUIController _uiController;
        private ICoreController _coreController;
        private RuntimeData _runtimeData;
        private IDataService _dataService;

        private void Awake() => DontDestroyOnLoad(this);

        public void Initialize(ICoreController coreController, IUIController uiController, IDataService dataService, RuntimeData runtimeData)
        {
            _coreController = coreController;
            _uiController = uiController;
            _dataService = dataService;
            _runtimeData = runtimeData;
        }

        public void IncreaseScore()
        {
            _runtimeData.IncreaseScore();
            _uiController.UpdateScore(_runtimeData.CurrentScore);
        }

        public void IncreaseLevel()
        {
            _runtimeData.IncreaseDifficulty();
            _uiController.UpdateLevel(_runtimeData.DifficultyLevel);
        }
        
        public void QuitGame() => Application.Quit();

        public void GameOver() => _uiController.GameOver(_dataService.SaveScore());
        public void RestartGame()
        {
            _coreController.Remove();
            Destroy(gameObject);
        }
    }
}