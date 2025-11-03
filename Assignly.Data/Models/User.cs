using Assignly.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Assignly.Data.Models
{
    public class User : IdentityUser
    {
        [EnumDataType(typeof(RoleEnum))]
        public RoleEnum Role { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<Notification>? Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<UserTask> UserTasks { get; set; } = new HashSet<UserTask>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
