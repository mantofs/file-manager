using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileManager
{
    public class Folder
    {
        public Folder(Folder parent, string name)
        {
            Name = name;
            _parent = parent;
            _directories = new List<Folder>();
            _items = new List<Item>();
        }
        private Folder _parent;  
        public string Name { get; private set; }
        private List<Folder> _directories;
        public IReadOnlyCollection<Folder> Directories => _directories;
        private List<Item> _items;
        public IReadOnlyCollection<Item> Items => _items;

        public void AddFolder(Folder Folder)
        {
            _directories.Add(Folder);
        }
        public void AddItem(string name, long length, DateTime create, DateTime write)
        {
            _items.Add(new Item(name, length, create, write));
        }
        public void AddDirectories(IEnumerable<Folder> directories)
        {
            _directories.AddRange(directories);
        }
        public Folder AddItems(IEnumerable<Item> Items)
        {
            if (Items != null && Items.Any())
                _items.AddRange(Items);
            return this;
        }
        private IEnumerable<Item> GetAllItems(Folder folder)
        {
            if (!folder.Directories.Any())
                return folder.Items;
            var items = new List<Item>();
            foreach (var directory in folder.Directories)
                items.AddRange(GetAllItems(directory));
            items.AddRange(folder.Items);
            return items;
        }
        public IEnumerable<Item> GetAllItems()
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

                foreach (var dtemDirectoryInfo in directoryInfo.EnumerateDirectories())
                    folder.AddFolder(FolderMount( folder,dtemDirectoryInfo.FullName));

                folder.AddItems(GetItems(directoryInfo.FullName));
                return folder;
            }

            private static IEnumerable<Item> GetItems(string path)
            {
                foreach (var ItemInfo in Directory.EnumerateFiles(path)
                                                 ?.Select(file => new FileInfo(file)))
                    yield return new Item(ItemInfo.Name, ItemInfo.Length, ItemInfo.CreationTime, ItemInfo.LastWriteTime);
            }
        }
    }
    public class Item
    {
        public Item(string name, long length, DateTime create, DateTime write)
        {
            Id = Guid.NewGuid();
            Name = name;
            Length = length;
            Create = create;
            Write = write;
        }
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public long Length { get; private set; }
        public DateTime Create { get; private set; }
        public DateTime Write { get; private set; }
    }

}
