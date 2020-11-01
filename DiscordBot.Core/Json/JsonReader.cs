using Newtonsoft.Json;
using System.IO;

namespace DiscordBot.Json
{
    public static class JsonReader
    {
        public static T Load<T>(string path, bool allowCreateNew = false) where T : new()
        {
            var file = new FileInfo(path);
            T obj;

            if (file.Exists)
            {
                string json = File.ReadAllText(path);

                try
                {
                    obj = JsonConvert.DeserializeObject<T>(json);
                }
                catch (JsonReaderException e)
                {
                    throw new JsonReaderException($"Unable to parse config file at {path}", e);
                }
            }
            else if (allowCreateNew)
            {
                obj = new T();
            }
            else
            {
                throw new FileNotFoundException();
            }

            Save(path, obj); // save deserialized configuration in order to remove unused entries and add new entries

            return obj;
        }

        public static void LoadToExisting<T>(string path, T target)
        {
            var file = new FileInfo(path);

            if (file.Exists)
            {
                string json = File.ReadAllText(path);
                
                try
                {
                    JsonConvert.PopulateObject(json, target);
                }
                catch (JsonReaderException e)
                {
                    throw new JsonReaderException($"Unable to parse config file at {path}", e);
                }
            }

            Save(path, target);
        }

        public static void Save(string path, object obj)
        {
            var file = new FileInfo(path);
            Directory.CreateDirectory(file.Directory.FullName);

            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);

            File.WriteAllText(path, json);
        }
    }

    public class JsonReader<T> where T : new()
    {
        public string SavePath { get; set; }

        public JsonReader(string path)
        {
            SavePath = path;
        }

        public JsonReader(string path, string localPath) : this(Path.Combine(path, localPath)) { }

        public T Load(bool allowCreateNew = false)
        {
            return JsonReader.Load<T>(SavePath, allowCreateNew);
        }

        public void LoadToExisting(T target)
        {
            JsonReader.LoadToExisting(SavePath, target);
        }

        public void Save(T obj)
        {
            JsonReader.Save(SavePath, obj);
        }
    }
}