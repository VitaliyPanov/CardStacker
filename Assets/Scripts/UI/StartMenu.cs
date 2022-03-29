using CardStacker.Data;
using CardStacker.General.Services;
using CardStacker.General.Services.SaveSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardStacker.UI
{
    internal sealed class StartMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _bootstrapperPrefab;
        private const string c_scoreKey = "1SASFlku&@!#ds%h";
        private const string c_startButton = "StartButton";
        private const string c_quitButton = "QuitButton";
        private const string c_scoreLabel = "ScoreValue";
        private const string c_rowsSlider = "RowsSlider";
        private const string c_rowsLabel = "RowsValue";
        private const string c_columnsSlider = "ColumnsSlider";
        private const string c_columnsLabel = "ColumnsValue";
        private StaticData _staticData;

        private Button _startButton;
        private Button _quitButton;
        private TextElement _scoreLabel;
        private SliderInt _rowsSlider;
        private SliderInt _columnsSlider;
        private TextElement _rowsLabel;
        private TextElement _columnsLabel;

        private void Awake()
        {
            _staticData = Resources.Load<StaticData>("Data/SceneStaticData");
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            _startButton = root.Q<Button>(c_startButton);
            _quitButton = root.Q<Button>(c_quitButton);
            _scoreLabel = root.Q<TextElement>(c_scoreLabel);
            _rowsSlider = root.Q<SliderInt>(c_rowsSlider);
            _columnsSlider = root.Q<SliderInt>(c_columnsSlider);
            _rowsLabel = root.Q<TextElement>(c_rowsLabel);
            _columnsLabel = root.Q<TextElement>(c_columnsLabel);
        }

        private void Start()
        {
            SaveDataRepository<ScoreData> scoreRepository =
                new SaveDataRepository<ScoreData>(SavingType.JsonCrypto, SavePaths.Score, c_scoreKey);
            var scoreRecord = scoreRepository.Load();
            _scoreLabel.text = scoreRecord != null ? $"{scoreRecord.Score}" : "0";
        }

        private void SetUpSettings(int rows, int columns)
        {
            _staticData.FieldHeight = rows;
            _staticData.FieldWidth = columns;
            SaveDataRepository<StaticData> dataRepository =
                new SaveDataRepository<StaticData>(SavingType.Json, SavePaths.StaticData);
            dataRepository.Save(_staticData, true);
        }

        private void QuitGame() => Application.Quit();

        private void StartGame()
        {
            SetUpSettings(_rowsSlider.value, _columnsSlider.value);
            Instantiate(_bootstrapperPrefab);
        }
        
        private void OnRowsSliderChangeEvent(ChangeEvent<int> evt) => _rowsLabel.text = $"{evt.newValue}";
        private void OnColumnsSliderChangeEvent(ChangeEvent<int> evt) => _columnsLabel.text = $"{evt.newValue}";

        private void OnEnable()
        {
            _startButton.clicked += StartGame;
            _quitButton.clicked += QuitGame;
            _rowsSlider.RegisterCallback<ChangeEvent<int>>(OnRowsSliderChangeEvent);
            _columnsSlider.RegisterCallback<ChangeEvent<int>>(OnColumnsSliderChangeEvent);
        }
        
        private void OnDisable()
        {
            _startButton.clicked -= StartGame;
            _quitButton.clicked -= QuitGame;
            _rowsSlider.UnregisterCallback<ChangeEvent<int>>(OnRowsSliderChangeEvent);
            _columnsSlider.UnregisterCallback<ChangeEvent<int>>(OnColumnsSliderChangeEvent);
        }
    }
}