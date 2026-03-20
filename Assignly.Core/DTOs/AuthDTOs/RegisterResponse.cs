using System.ComponentModel.DataAnnotations;
using Assignly.Data.Enums;

namespace Assignly.Core.DTOs.AuthDTOs;

public class RegisterResponse
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    [EnumDataType(typeof(RoleEnum))]
    public int Role { get; set; }
}
