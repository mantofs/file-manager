using System;

namespace FileManager.Domain
{
    public class File
    {
        public File(string name, long length, DateTime create, DateTime write)
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
