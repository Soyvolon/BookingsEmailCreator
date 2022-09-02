using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingsEmailCreator.Api;

[Route("/challenge")]
[ApiController]
public class ChallengeController : Controller
{
    [Route("/msauth")]
    [HttpGet]
    [Authorize(AuthenticationSchemes = "MSAuth")]
    public Task<IActionResult> GetMeEndpointAsync(string redirect)
    {
        return Task.FromResult<IActionResult>(Redirect(redirect));
    }
}
