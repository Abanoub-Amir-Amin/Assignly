using System.ComponentModel.DataAnnotations;
using Assignly.Data.Enums;

namespace Assignly.Core.DTOs.AuthDTOs;

public class RegisterRequest
{
    public string UserName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [RegularExpression(
        @"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,16}$",
        ErrorMessage = "password must contain 1 number (0-9)\r\npassword must contain 1 uppercase letters\r\npassword must contain 1 lowercase letters\r\npassword must contain 1 non-alpha numeric number\r\npassword is 8-16 characters with no space"
    )]
    public string Password { get; set; }
    public RoleEnum Role { get; set; }
}
