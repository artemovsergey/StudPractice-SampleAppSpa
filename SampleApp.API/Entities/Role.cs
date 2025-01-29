using SampleApp.Repositories;

namespace SampleApp.API.Entities;

public class Role : Base, IEntity
{
  public required string Name {get; set;}   
}