using System.IO;
using System.Text.Json;
using FileManager.Domain;
namespace FileManager.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = Directory.GetCurrentDirectory();

            var foldersAndfiles = Folder.Factory.Construct(Path.Combine( directory,"files"));
        
            using(StreamWriter file = new StreamWriter(Path.Combine(directory, "hierarchicalFile.json"))){
               file.Write(JsonSerializer.Serialize(foldersAndfiles));
            };
        }
    }
}
