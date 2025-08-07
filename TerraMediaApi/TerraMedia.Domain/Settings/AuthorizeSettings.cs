namespace TerraMedia.Domain.Settings;

public class AuthorizeSettings
{
    public string Secret { get; set; } = string.Empty;
    public int Expires { get; set; }
}
