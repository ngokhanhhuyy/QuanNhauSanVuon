namespace QuanNhauSanVuon.Services.Exceptions;

public class AuthenticationException : Exception
{
    public AuthenticationException()
    {
    }

    public AuthenticationException(string message) : base(message)
    {
    }
}