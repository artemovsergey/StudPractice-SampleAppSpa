using System.Text.Json.Serialization;
using SampleApp.Repositories;

namespace SampleApp.API.Entities;

public class Micropost : Base, IEntity
{
    public string Content {get; set;} = string.Empty;
    public int UserId {get; set;}
    
    //[JsonIgnore]
    public User? User {get; set;}
}