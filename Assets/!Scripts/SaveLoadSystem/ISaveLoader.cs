namespace Shir0.SaveLoadSystem
{
    public interface ISaveLoader<T> where T : class
    {
        string FilePath { get; }
        void Save(T obj);
        T Load(string path);
    }
}