using System.IO;
using System.Xml.Serialization;

namespace Print.Templates.Core
{
    public static class Utils
    {
        public static string SerializeToXml<T>(T obj)
        {
            if (obj == null) return null;
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, obj);
                ms.Position = 0;
                using (var reader = new StreamReader(ms))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static T DeserializeXml<T>(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(T));
            var result     = serializer.Deserialize(stream);
            if (result is T node)
            {
                return node;
            }
            return default(T);
        }

        public static T DeserializeXml<T>(string xml)
        {
            if (xml == null) return default(T);
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms))
                {
                    sw.Write(xml);
                    sw.Flush();
                    ms.Position = 0;
                    return DeserializeXml<T>(ms);
                }
            }
        }
    }
}
