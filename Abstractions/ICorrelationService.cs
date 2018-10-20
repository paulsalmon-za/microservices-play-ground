namespace Abstractions
{
    public interface ICorrelationService
    {
        string GenerateIdIfNull(string correlationGuid);
    }
}