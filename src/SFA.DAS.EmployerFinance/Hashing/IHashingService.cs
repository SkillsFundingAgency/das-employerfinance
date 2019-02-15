namespace SFA.DAS.EmployerFinance.Hashing
{
    public interface IHashingService
    {
        long DecodeValue(string input);
        bool TryDecodeValue(string input, out long output);
    }
}