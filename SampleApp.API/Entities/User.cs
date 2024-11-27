using System.ComponentModel.DataAnnotations;

namespace SampleApp.API.Entities;

public class User : Base{
        
    [MinLength(5,ErrorMessage = "Минимальное длина имени 5")]
    [SampleApp.API.Validations.MaxLength(10)]
    public string Name {get ;set;} = String.Empty;
    public string Login {get; set;} = String. Empty;
    public byte[] PasswordHash {get; set;}
    public byte[] PasswordSalt {get; set;}
}