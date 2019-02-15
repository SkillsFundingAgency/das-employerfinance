namespace SFA.DAS.EmployerFinance.Hashing
{
    public interface IHashingService
    {
        bool TryDecodeValue(string input, out long output);
    }
}