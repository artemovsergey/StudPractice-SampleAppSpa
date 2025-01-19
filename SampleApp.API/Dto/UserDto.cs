namespace SampleApp.API.Dto;

public record UserRecordDto {
  public required string Login {get; init;}
  public required string Password {get; init;}
};