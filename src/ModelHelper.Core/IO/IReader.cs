namespace ModelHelper.IO
{
    public interface IReader<T>
    {
        T Read(string path);
        // T Read(FileInfo file);
    }
}