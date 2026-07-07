using System;
using System.IO;
using System.Text.Json;

namespace PostureGuard.Libraries
{
    public static class DataLibrary
    {
        private static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PostureGuard");
        private static readonly string FilePath = Path.Combine(FolderPath, "config.json");

        public static void Save<T>(T data)
        {
            if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public static T? Load<T>() where T : class
        {
            if (!File.Exists(FilePath)) return null;
            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}