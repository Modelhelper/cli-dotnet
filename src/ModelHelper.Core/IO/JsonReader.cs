using System.IO;
using Newtonsoft.Json;

namespace ModelHelper.IO
{
    public class JsonReader<T> : IReader<T> where T: class, new()
    {
        public T Read(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new T();
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found");

            }

            var json = File.ReadAllText(path);
            var item = JsonConvert.DeserializeObject<T>(json);
            // var deserializer = new YamlDotNet.Serialization.Deserializer();

            // var item = deserializer.Deserialize<T>(json);
            return item;

        }
    }

    public class JsonWriter<T> : IWriter<T> where T: class, new()
    {
        public void Write(string path, T item)
        {
           var settings = new JsonSerializerSettings();
            
            var json = JsonConvert.SerializeObject(item, Formatting.Indented);
            System.IO.File.WriteAllText(path, json);
        }
        
    }
}