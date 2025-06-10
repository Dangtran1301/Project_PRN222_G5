using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;
using Project_PRN222_G5.BusinessLogic.Exceptions;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Controllers;

[AllowAnonymous]
public class AuthController(IAuthService authService, ICookieService cookieService, ILogger<AuthController> logger) : Controller
{
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid) return View(loginRequest);

        try
        {
            var response = await authService.LoginAsync(loginRequest);

            // Gắn AccessToken + RefreshToken vào cookie
            await cookieService.SetAuthCookiesAsync(
                loginRequest.Username,
                response.AccessToken,
                response.RefreshToken
            );

            TempData["SuccessMessage"] = "Login successfully!";

            var user = await authService.GetUserByUsernameAsync(loginRequest.Username);
            if (user.Role == Role.Admin)
            {
                return RedirectToPage(PageRoutes.Users.Index);
            }

            return RedirectToAction("Home", "Home");
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                foreach (var msg in error.Value)
                {
                    ModelState.AddModelError(error.Key, msg);
                }
            }
            return View(loginRequest);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Unexpected error: " + ex.Message);
            return View(loginRequest);
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        cookieService.RemoveAuthCookies();
        TempData["SuccessMessage"] = "Logged out successfully!";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Register()
    {
        var model = new RegisterUserRequest();
        ViewBag.Roles = Enum.GetValues(typeof(Role))
            .Cast<Role>()
            .Where(r => r != Role.Admin)
            .Select(r => new SelectListItem { Value = r.ToString(), Text = r.ToString() })
            .ToList();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserRequest input)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = Enum.GetValues(typeof(Role))
                .Cast<Role>()
                .Where(r => r != Role.Admin)
                .Select(r => new SelectListItem { Value = r.ToString(), Text = r.ToString() })
                .ToList();
            return View(input);
        }

        try
        {
            await authService.CreateAsync(input);
            TempData["SuccessMessage"] = "Registered successfully. Please log in.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(input);
        }
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> RefreshToken()
    {
        var accessToken = Request.Cookies["AccessToken"];
        var refreshToken = User.FindFirst("RefreshToken")?.Value;
        var userIdClaim = User.FindFirst("uid")?.Value;

        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
        {
            logger.LogWarning("Missing tokens");
            return Unauthorized();
        }

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            logger.LogWarning("Invalid user ID");
            return Unauthorized();
        }

        try
        {
            var request = new RefreshTokenRequest
            {
                UserId = userId,
                RefreshToken = refreshToken
            };

            var response = await authService.RefreshTokenAsync(request);
            await cookieService.SetAuthCookiesAsync(User.Identity!.Name!, response.AccessToken, response.RefreshToken);

            return Json(new { response.AccessToken });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Refresh token failed");
            return Unauthorized();
        }
    }
}