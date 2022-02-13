using System.Collections.Generic;

namespace CloudDataProtection.Functions.BackupDemo.Triggers.Dto.Output
{
    public class FileDestinationOutput
    {
        public IEnumerable<FileDestinationOutputEntry> Sources { get; set; }
    }

    public class FileDestinationOutputEntry
    {
        public int FileDestination { get; set; }
        public string Description { get; set; }

        public FileDestinationOutputEntry(int fileDestination, string description)
        {
            FileDestination = fileDestination;
            Description = description;
        }
    }
}