using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace LeseEulenBibliothek.Core
{
    public static class SerializationHelper
    {
        public static T LoadFileData<T>(string filename) where T : class, new()
        {
            if (!File.Exists(filename))
                return new T();
            return JsonSerializer.Deserialize<T>(File.ReadAllBytes(filename));
        }

        public static void SaveFileData<T>(string filename, T data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IgnoreNullValues = true
            };
            using var writer = new Utf8JsonWriter(fileStream, new JsonWriterOptions() { Indented = true });
            JsonSerializer.Serialize<T>(writer, data, options);
        }
    }
}
