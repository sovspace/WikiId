using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class MediaFile : BaseEntity
    {
        public string Path { get; set; }
        public FileType Type { get; set; }
    }
}
