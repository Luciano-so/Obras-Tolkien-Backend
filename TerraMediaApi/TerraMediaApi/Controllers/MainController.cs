using Microsoft.AspNetCore.Mvc;

namespace TerraMedia.Api.Controllers;

[Route("api")]
[ApiController]
public class MainController : ControllerBase
{
    private Guid _UserId;
    protected Guid UserId
    {
        get
        {
            var claims = User.Claims.FirstOrDefault(t => t.Type == "userId");
            if (claims != null)
            {
                _UserId = new Guid(claims.Value);
            }
            return _UserId;
        }

    }
}
