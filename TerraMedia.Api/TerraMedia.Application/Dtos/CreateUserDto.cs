﻿namespace TerraMedia.Application.Dtos;

public class CreateUserDto
{
    public string Name { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
