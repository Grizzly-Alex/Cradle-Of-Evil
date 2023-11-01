namespace Interfaces
{
    public interface IItemPool<T> where T : class
    {
        public T Create();
        public void Get();
        public void Release();
        public void Destroy();
    }
}
