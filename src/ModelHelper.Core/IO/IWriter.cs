namespace ModelHelper.IO
{
    public interface IWriter<T>
    {
        void Write(string path, T item);
    }
}