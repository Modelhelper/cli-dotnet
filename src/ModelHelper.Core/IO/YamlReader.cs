using System.Collections.Generic;
using System.IO;
using System.Text;
using ModelHelper.Core;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

namespace ModelHelper.IO
{
    public class YamlReader<T> where T : class //, new()
    {
        public T Read(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found");

            }

            var yaml = File.ReadAllText(path);
            var deserializer = new YamlDotNet.Serialization.Deserializer();

            var item = deserializer.Deserialize<T>(yaml);
            return item;

        }
    }

    public class YamlWriter<T> where T : class //, new()
    {
        public void Write(string path, T item)
        {
            // if (!File.Exists(path))
            // {
            //     throw new FileNotFoundException("File not found");

            // }
            var fileInfo = new FileInfo(path);

            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            
            var sb = new SerializerBuilder()
                .WithEventEmitter(next => new FlowStyleStringSequences(next))
                .Build();

            var yaml = sb.Serialize(item);

            File.WriteAllText(path, yaml, Encoding.UTF8);
            
        }
    }
    // public class ConfigurationReader
    // {
    //     public ModelHelperConfiguration Read(string path)
    //     {
    //         var sb = new SerializerBuilder()
    //                 .WithEventEmitter(next => new FlowStyleStringSequences(next))
    //                 .Build();


    //         var serializer = new YamlDotNet.Serialization.Serializer();
    //         var des = new YamlDotNet.Serialization.Deserializer();

    //         //var deserializer = new 

    //         return null;
    //     }
    // }

    public class FlowStyleStringSequences : ChainedEventEmitter
    {
        public FlowStyleStringSequences(IEventEmitter nextEmitter)
            : base(nextEmitter) { }

        public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
        {
            if (typeof(IEnumerable<string>).IsAssignableFrom(eventInfo.Source.Type))
            {
                eventInfo = new SequenceStartEventInfo(eventInfo.Source)
                {
                    Style = SequenceStyle.Flow
                };
            }

            nextEmitter.Emit(eventInfo, emitter);
        }
    }

    
}