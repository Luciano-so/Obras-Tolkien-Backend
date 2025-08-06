using System.Diagnostics.CodeAnalysis;

namespace TerraMedia.Domain.Settings
{
    [ExcludeFromCodeCoverage]
    public class AuthorizeSettings
    {
        public string Secret { get; set; } = string.Empty;
        public int Expires { get; set; }
    }
}
