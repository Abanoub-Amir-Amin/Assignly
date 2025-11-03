using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignly.Data.Models
{
    public class Organization
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string AdminId { get; set; }
        public User Admin { get; set; }
        public ICollection<User> Users { get; set; } = new HashSet<User>();
        public ICollection<Workspace>? Workspaces { get; set; } = new HashSet<Workspace>();
    }
}
