namespace WebApplication1.DTO.AuthDTO;

public class LoginResponse
{
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
}