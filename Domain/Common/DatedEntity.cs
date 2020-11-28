using System;

namespace Domain.Common
{
    public abstract class DatedEntity : BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
