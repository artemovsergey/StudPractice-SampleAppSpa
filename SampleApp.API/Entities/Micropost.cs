namespace SampleApp.API.Entities;

public class Micropost : Base
{
    public string Content {get; set;} = string.Empty;
    public int UserId {get; set;}
    public User? User;
}