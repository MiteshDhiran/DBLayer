using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public static class DataContractSerializerHelper
    {
        public static string DataContractSerialize<T>(T objectToSerialize)
        {
            var retVal = "";
            using (var memStm = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memStm, objectToSerialize);
                memStm.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(memStm))
                {
                    retVal = streamReader.ReadToEnd();
                }
            }
            return retVal;
            
        }
        
        public static T CloneUsingJSONWithString<T>(T objectToClone) where T: class
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(memoryStream, objectToClone);
                memoryStream.Flush();
                memoryStream.Position = 0;
                var deserializedString = Encoding.UTF8.GetString(memoryStream.ToArray());
                using (MemoryStream deserializeMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(deserializedString)))
                {
                    deserializeMemoryStream.Position = 0;
                    var deSerializer = new DataContractJsonSerializer(typeof(T));
                    var disconnectedRecords =  deSerializer.ReadObject(deserializeMemoryStream) as T;
                    return disconnectedRecords;
                }
            }
        }

        public static T DataContractJSONDeserializer<T>(string deserializedString) where T: class
        {
                return JsonSerializer.Deserialize<T>(deserializedString);
        }

        public static string DataContractJSONSerialize<T>(T rec) where T: class
        {
            return JsonSerializer.Serialize<T>(rec);
        }

        
        public static T DataContractDeserializer<T>(string dataContractString) 
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(dataContractString)))
            {
                var readerQuotas = new XmlDictionaryReaderQuotas();
                readerQuotas.MaxStringContentLength = 2147483647;
                readerQuotas.MaxBytesPerRead = 2147483647;
                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(memoryStream
                    , Encoding.UTF8, readerQuotas, null))
                {
                    DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));
                    var retVal = dataContractSerializer.ReadObject(reader);
                    return (T)retVal ;
                }

            }
        }
        
        
        
    }
}
