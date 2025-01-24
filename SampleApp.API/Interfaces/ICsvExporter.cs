using SampleApp.API.Entities;

namespace SampleApp.API.Interfaces;

public interface ICsvExporter
{
    byte[] ExportUsersToCsv(List<User> users);
}