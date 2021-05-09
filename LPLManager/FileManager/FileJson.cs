using System;
using System.IO;
using System.Text.Json;

namespace LPLManager.FileManager
{
    public static class FileJson<T>
    {
        public static T Read(string readPath)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(readPath));
            }
            catch (Exception e)
            {
                throw new Exception("Error: ", e);
            }
        }

        public static void Write(string writePath, T obj)
        {
            try
            {
                File.WriteAllText(writePath, JsonSerializer.Serialize(obj, new JsonSerializerOptions() { WriteIndented = true }));
            }
            catch (Exception e)
            {
                throw new Exception("Error: ", e);
            }
        }
    }
}
