namespace CardStacker.General.Services.SaveSystem
{
    public interface IData<T>
    {
        void Save(T data, string path, string key);
        T Load(string path, string key);
        void LoadOverwrite(string path, T target, string key);
    }
}