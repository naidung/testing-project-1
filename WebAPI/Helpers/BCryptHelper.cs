namespace WebAPI.Helpers;

public class BCryptHelper
{
    public BCryptHelper()
    {
        
    }

    public string GenHashCode(string plain)
    {
        return BCrypt.Net.BCrypt.HashPassword(plain);
    }

    public bool VerifyHashAndPlain(string dbPassword, string plain)
    {
        return BCrypt.Net.BCrypt.Verify(plain, dbPassword);
    }
}