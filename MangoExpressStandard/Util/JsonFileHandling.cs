using System;
using System.IO;
using Newtonsoft.Json;

namespace MangoExpressStandard.Util
{
    public static class JsonFileHandling
    {
        public static void ObjectToJsonFile<T>(T thisObject, string jsonFilePath)
        {
            var dir = Path.GetDirectoryName(jsonFilePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (StreamWriter file = File.CreateText(jsonFilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, thisObject);
            }
        }

        public static T JsonFileToObject<T>(string jsonFilePath)
        {
            T tObject;

            using (StreamReader file = File.OpenText(jsonFilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                tObject = (T)serializer.Deserialize(file, typeof(T));
            }

            return tObject;
        }
    }
}
