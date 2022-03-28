using CardStacker.General.Controllers;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardStacker.UI
{
    public sealed class GameHUD : MonoBehaviour
    {
        private const string c_score = "ScoreCounter";
        private const string c_difficulty = "DifficultyCounter";
        private const string c_quitButton = "QuitButton";
        private const string c_restartButton = "RestartButton";
        private const string c_congArea = "Congratulations";
        private const string c_gameOverLabel = "GameOverLabel";
        private TextElement _difficultyLevelText;
        private TextElement _scoreText;
        private IControllersMediator _mediator;
        private Button _quitButton;
        private Button _restartButton;
        private VisualElement _congArea;
        private TextElement _gameOverText;

        public void Construct(IControllersMediator mediator) => _mediator = mediator;

        private void Awake()
        {
             var root = GetComponent<UIDocument>().rootVisualElement;
            _quitButton = root.Q<Button>(c_quitButton);
            _restartButton = root.Q<Button>(c_restartButton);
            _scoreText = root.Q<TextElement>(c_score);
            _difficultyLevelText = root.Q<TextElement>(c_difficulty);
            _congArea = root.Q<VisualElement>(c_congArea);
            _gameOverText = root.Q<TextElement>(c_gameOverLabel);
        }

        private void Start()
        {
            _congArea.style.display = DisplayStyle.None;
        }

        private void OnEnable()
        {
            _quitButton.clicked += QuitGame;
            _restartButton.clicked += RestartGame;
        }

        private void RestartGame() => _mediator.RestartGame();

        private void QuitGame() => _mediator.QuitGame();


        public void UpdateScore(int value) => _scoreText.text = $"{value}";

        public void UpdateLevel(int value) => _difficultyLevelText.text = $"{value}";

        private void OnDisable()
        {
            _quitButton.clicked -= QuitGame;
            _restartButton.clicked -= RestartGame;
        }

        public void GameOver(bool isNewRecord)
        {
            _congArea.style.display = DisplayStyle.Flex;
            _gameOverText.text = isNewRecord ? "NEW RECORD!!!" : "GAME OVER!";
        }
    }
}
