using System.Security.Authentication.ExtendedProtection;

namespace SimpleAccount.Services
{
    public interface ICache<Object, Identifier>
    {
        Object Retrieve(bool invalidate, Identifier identifier);
    }
}