using System.ComponentModel.DataAnnotations;

namespace SampleApp.API.Dto;

public class UserUpdateDto
{
    public int Id {get; set;} 

    [MinLength(5,ErrorMessage = "Минимальное длина имени 5")]
    [SampleApp.API.Validations.UserNameMaxLengthValidation(10)]
    public string Login {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;
}