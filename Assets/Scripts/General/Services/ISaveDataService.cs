namespace CardStacker.General.Services
{
    public interface ISaveDataService<T>
    {
        void Save(T saveData, bool overWrite);
        public T Load();
        void Overwrite(T data);
    }
    public enum SavingType
    {
        Json,
        JsonCrypto,
        XML
    }
}