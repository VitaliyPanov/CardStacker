using CardStacker.Data;

namespace CardStacker.General.Services
{
    public interface IDataService
    {
        void Load();
        bool SaveScore();
        StaticData StaticData { get; }
        RuntimeData RuntimeData { get; }
        CardData CardData { get; }
        UIData UIData { get; }
    }
}