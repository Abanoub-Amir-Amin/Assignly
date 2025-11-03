using System.ComponentModel.DataAnnotations;

namespace Assignly.Data.Models
{
    public class Workspace
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<Module>? Modules { get; set; } = new HashSet<Module>();
    }
}
