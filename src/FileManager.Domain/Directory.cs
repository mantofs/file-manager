using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileManager.Domain
{
    public class Folder
    {
        public Folder(Folder parent, string name)
        {
            Name = name;
            _parent = parent;
            _directories = new List<Folder>();
            _files = new List<Domain.File>();
        }
        private Folder _parent;  
        public string Name { get; private set; }
        private List<Folder> _directories;
        public IReadOnlyCollection<Folder> Directories => _directories;
        private List<Domain.File> _files;
        public IReadOnlyCollection<Domain.File> Files => _files;

        public void AddFolder(Folder Folder)
        {
            _directories.Add(Folder);
        }
        public void AddItem(string name, long length, DateTime create, DateTime write)
        {
            _files.Add(new Domain.File(name, length, create, write));
        }
        public void AddDirectories(IEnumerable<Folder> directories)
        {
            _directories.AddRange(directories);
        }
        public Folder AddItems(IEnumerable<Domain.File> Items)
        {
            if (Items != null && Items.Any())
                _files.AddRange(Items);
            return this;
        }
        private IEnumerable<Domain.File> GetAllItems(Folder folder)
        {
            if (!folder.Directories.Any())
                return folder.Files;
            var items = new List<Domain.File>();
            foreach (var directory in folder.Directories)
                items.AddRange(GetAllItems(directory));
            items.AddRange(folder.Files);
            return items;
        }
        public IEnumerable<Domain.File> GetAllItems()
        {
            return GetAllItems(this);
        }
        public static class Factory
        {
            public static Folder Construct(string path)
            {
                if (!Directory.Exists(path))
                    throw new Exception("O diretório informado não existe.");
                return FolderMount(null, path);
            }

            private static Folder FolderMount(Folder parent, string path)
            {
                var directoryInfo = new DirectoryInfo(path);
                var folder = new Folder(parent, directoryInfo.Name);

                if (!directoryInfo.EnumerateDirectories().Any())
                    return folder.AddItems(GetItems(directoryInfo.FullName));

                foreach (var directory in directoryInfo.EnumerateDirectories())
                    folder.AddFolder(FolderMount( folder,directory.FullName));

                folder.AddItems(GetItems(directoryInfo.FullName));
                return folder;
            }

            private static IEnumerable<Domain.File> GetItems(string path)
            {
                foreach (var fileInfo in Directory.EnumerateFiles(path)
                                                 ?.Select(file => new FileInfo(file)))
                    yield return new Domain.File(fileInfo.Name, fileInfo.Length, fileInfo.CreationTime, fileInfo.LastWriteTime);
            }
        }
    }

}
