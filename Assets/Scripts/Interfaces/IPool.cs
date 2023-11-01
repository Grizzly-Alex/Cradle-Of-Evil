namespace Interfaces
{
    public interface IPool<T> where T : class
    {
        public T Get();
        public void Release(T item);
        public void Clear();
    }
}
