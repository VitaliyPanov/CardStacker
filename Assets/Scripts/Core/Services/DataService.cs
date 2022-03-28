using CardStacker.Data;
using CardStacker.General.Services;
using CardStacker.General.Services.SaveSystem;
using UnityEngine;

namespace CardStacker.Core.Services
{
    internal sealed class DataService : IDataService
    {
        private const string c_scoreKey = "1SASFlku&@!#ds%h";
        public StaticData StaticData { get; private set; }
        public RuntimeData RuntimeData { get; private set; }
        public CardData CardData { get; private set; }
        public UIData UIData { get; private set; }

        public void Load()
        {
            StaticData = Resources.Load<StaticData>("Data/SceneStaticData");
            SaveDataRepository<StaticData> staticDataRepository =
                new SaveDataRepository<StaticData>(SavingType.Json, SavePaths.StaticData);
            staticDataRepository.Overwrite(StaticData);
            CardData = Resources.Load<CardData>("Data/CardData");
            UIData = Resources.Load<UIData>("Data/UIData");
            RuntimeData = new RuntimeData();
            InitializeRuntimeData();
        }

        public bool SaveScore()
        {
            SaveDataRepository<ScoreData> scoreRepository =
                new SaveDataRepository<ScoreData>(SavingType.JsonCrypto, SavePaths.Score, c_scoreKey);
            var scoreRecord = scoreRepository.Load();
            if (scoreRecord == null)
            {
                scoreRepository.Save(new ScoreData() {Score = RuntimeData.CurrentScore});
                return true;
            }
            if (scoreRecord.Score < RuntimeData.CurrentScore)
            {
                scoreRepository.Save(new ScoreData() {Score = RuntimeData.CurrentScore});
                return true;
            }
            return false;
        }

        private void InitializeRuntimeData()
    {
        Vector2 cardSize = CardData.CardPrefab.GetComponentInChildren<SpriteRenderer>().size;
        RuntimeData.SetCardSize(cardSize.x, cardSize.y);
        RuntimeData.SetComplication(StaticData.MaxRedChanceRate, StaticData.DifficultyStepMoves);
    }
}

}