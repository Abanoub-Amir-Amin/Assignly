using System.ComponentModel.DataAnnotations;
using Assignly.Data.Enums;

namespace Assignly.Core.DTOs.AuthDTOs;

public class RegisterRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public RoleEnum Role { get; set; }
}
