using System;

namespace FileManager
{
    class Program
  {
    static void Main(string[] args)
    {
      var directory = Folder.Factory.Construct("[directory]");
      
      var str = directory.GetAllItems();

      Console.Write(str);

    }
  }
}