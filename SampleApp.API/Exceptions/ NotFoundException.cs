namespace SampleApp.API.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key) : base($"{name}({key}) not fount")
    {

    }
}
