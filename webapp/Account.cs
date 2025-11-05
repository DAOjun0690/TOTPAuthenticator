namespace TOTPAuthenticatorWeb;

public class Account
{
    public string Name { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string? Issuer { get; set; }
    public string? CustomString { get; set; }
}
