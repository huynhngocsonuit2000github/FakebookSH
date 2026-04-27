namespace Fakebook.Auth.Application.Boundary.Security;

public interface IPasswordService
{
    string Hash(string password);

    bool Verify(string password, string passwordHash);
}