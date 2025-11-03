namespace Assignly.Data.Models
{
    public class UserTask
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid TaskId { get; set; }
        public Task Task { get; set; }
    }
}
