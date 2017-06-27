using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using monolithic_shop_core.Helpers;

namespace monolithic_shop_core.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet("~/signin")]
        public IActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("SignIn", HttpContext.GetExternalProviders());
        }

        [HttpPost("~/signin")]
        public IActionResult SignIn([FromForm] string provider, [FromForm] string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }

            if (!HttpContext.IsProviderSupported(provider))
            {
                return BadRequest();
            }

            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, provider);
        }

        [HttpGet("~/signout"), HttpPost("~/signout")]
        public IActionResult SignOut()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
