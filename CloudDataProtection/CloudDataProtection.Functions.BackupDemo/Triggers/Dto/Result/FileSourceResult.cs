using System.Collections.Generic;

namespace CloudDataProtection.Functions.BackupDemo.Triggers.Dto.Result
{
    public class FileDestinationResult
    {
        public IEnumerable<FileDestinationResultEntry> Sources { get; set; }
    }

    public class FileDestinationResultEntry
    {
        public int FileDestination { get; set; }
        public string Description { get; set; }

        public FileDestinationResultEntry(int fileDestination, string description)
        {
            FileDestination = fileDestination;
            Description = description;
        }
    }
}