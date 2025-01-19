using System.ComponentModel.DataAnnotations;

namespace SampleApp.API.Entities;

public class User : Base
{

    [MinLength(5, ErrorMessage = "Минимальная длина имени: 5")]
    [SampleApp.API.Validations.UserNameMaxLengthValidation(10)]
    public string Name { get; set; } = String.Empty;

    public required string Login { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
}