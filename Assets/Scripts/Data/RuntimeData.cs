namespace CardStacker.Data
{
    public sealed class RuntimeData
    {
        private readonly float _cardOffset = 1f;
        public float CardWight { get; private set; }
        public float CardHeight { get; private set; }
        public float HorizontalOffset => CardWight + _cardOffset;
        public float VerticalOffset => CardHeight + _cardOffset;
        public float RedChanceRate { get; private set; }
        public int DifficultyLevel { get; private set; }
        public int DifficultyStepValue { get; private set; }
        public int CurrentScore { get; private set; }

        public RuntimeData() => DifficultyLevel = 1;

        public void IncreaseDifficulty() => DifficultyLevel++;

        public void SetComplication(float redCardChanceRate, int difficultyStep)
        {
            RedChanceRate = redCardChanceRate;
            DifficultyStepValue = difficultyStep;
        }

        public void SetCardSize(float wight, float height)
        {
            CardWight = wight;
            CardHeight = height;
        }

        public void IncreaseScore() => CurrentScore++;
    }
}