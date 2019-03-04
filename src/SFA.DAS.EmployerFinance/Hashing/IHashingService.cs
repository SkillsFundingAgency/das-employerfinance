namespace SFA.DAS.EmployerFinance.Hashing
{
    public interface IHashingService
    {
        bool TryDecodeLong(string input, out long output);
    }
}