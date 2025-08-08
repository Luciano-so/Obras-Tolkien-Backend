using TerraMedia.Domain.Base;
using TerraMedia.Domain.Extensions;
using TerraMedia.Domain.Validations;

namespace TerraMedia.Domain.Entities;

public class User : Entity
{
    protected User() { }
    public bool Active { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Login { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;

    public override void Validate()
    {
        Validation.ValidateIfEmpty(Name, "O campo Nome do usuário não pode estar vazio.");
        Validation.ValidateLength(Name, 2, 250, "O nome do usuário deve conter entre {0} e {1} caracteres.");

        Validation.ValidateIfEmpty(Login, "O campo login do usuário não pode estar vazio.");
        Validation.ValidateLength(Login, 2, 50, "O login deve conter entre {0} e {1} caracteres.");

        Validation.ValidateIfFalse(CreatedAt > DateTime.MinValue, "A data de registro deve ser válida.");

        ValidPassword(Password);
    }

    public void Activate() => Active = true;

    public void Deactivate() => Active = false;

    public void ChangeStatus(bool status) => Active = status;


    public void ValidPassword(string newPassword)
    {
        Validation.ValidateIfEmpty(newPassword, "O campo Senha não pode estar vazio.");
        Validation.ValidateLength(newPassword, 6, 100, "A senha deve conter entre {0} e {1} caracteres.");

        Password = newPassword;
    }

    public void Encrypt() => Password = Password.Encrypt();

    public static class Factory
    {
        public static User Create(string name, string login, string password)
        {
            var user = new User
            {
                Name = name.Trim().ToUpper(),
                Login = login.Trim().ToUpper(),
                Password = password,
                CreatedAt = DateTime.UtcNow,
            };
            user.Activate();
            user.Validate();
            user.Encrypt();
            return user;
        }
    }
}
