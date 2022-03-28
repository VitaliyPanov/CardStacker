using CardStacker.Data;

namespace CardStacker.General.Controllers
{
    public interface IUIController : IController
    {
        void Initialize(UIData data);
        void UpdateScore(int value);
        void UpdateLevel(int value);
        void GameOver(bool isNewRecord);
    }
}