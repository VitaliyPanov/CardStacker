using CardStacker.Data;
using CardStacker.General.Controllers;
using CardStacker.General.Services;
using CardStacker.UI;
using UnityEngine;

namespace CardStacker.Core.GameControllers
{
    public sealed class UIController : IUIController
    {
        private readonly IDataService _dataService;
        private readonly IControllersMediator _mediator;
        private GameHUD _hud;
        public UIController(IDataService dataService, IControllersMediator mediator)
        {
            _dataService = dataService;
            _mediator = mediator;
        }
        
        public void Initialize(UIData data)
        {
            _hud = Object.Instantiate(_dataService.UIData.GameHUD).GetOrAddComponent<GameHUD>();
            _hud.Construct(_mediator);
        }

        public void UpdateScore(int value) => _hud.UpdateScore(value);
        public void UpdateLevel(int value) => _hud.UpdateLevel(value);
        public void GameOver(bool isNewRecord) => _hud.GameOver(isNewRecord);
    }
}