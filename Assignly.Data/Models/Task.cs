using Assignly.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Assignly.Data.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        [Required]
        public string Description { get; set; }
        [EnumDataType(typeof(PriorityEnum))]
        public PriorityEnum Priority { get; set; }
        public DateTime DueDate { get; set; }
        public Guid ModuleId { get; set; }
        public Module Module { get; set; }
        public ICollection<UserTask> UserTasks { get; set; } = new HashSet<UserTask>();
        public ICollection<Attachment>? Attachments { get; set; } = new HashSet<Attachment>();
    }
}
