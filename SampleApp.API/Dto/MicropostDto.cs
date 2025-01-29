namespace SampleApp.API.Dto;

public class MicropostDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public UserRecordDto? User { get; set; } // Используем UserRecordDto вместо сущности User
}