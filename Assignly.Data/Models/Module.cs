using System.ComponentModel.DataAnnotations;

namespace Assignly.Data.Models
{
    public class Module
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }
        public ICollection<Task>? Tasks { get; set; } = new HashSet<Task>();
    }
}
