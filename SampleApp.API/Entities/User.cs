using System.ComponentModel.DataAnnotations;

namespace SampleApp.API.Entities;

public class User : Base{
        

    public string Name {get ;set;} = String.Empty;

    [MinLength(5,ErrorMessage = "Минимальное длина имени 5")]
    [SampleApp.API.Validations.MaxLength(10)]
    public string Login {get; set;} = String. Empty;
    public byte[] PasswordHash {get; set;} = default!;
    public byte[] PasswordSalt {get; set;} = default!;
    public string Token {get; set;} = string.Empty;
}