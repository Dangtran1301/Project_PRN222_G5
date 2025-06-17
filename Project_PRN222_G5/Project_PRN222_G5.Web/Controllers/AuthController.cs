using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.DataAccess.DTOs.Users.Requests;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.DataAccess.Exceptions;
using Project_PRN222_G5.DataAccess.Interfaces.Service;

namespace Project_PRN222_G5.Web.Controllers;

[AllowAnonymous]
public class AuthController(
    IAuthService authService,
    IJwtService jwtService,
    ICookieService cookieService,
    IAuthenticatedUserService authenticatedUserService,
    ILogger<AuthController> logger) : Controller
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
            var user = await authService.GetUserByUsernameAsync(loginRequest.Username);

            await cookieService.SetAuthCookiesAsync(
                user,
                response.AccessToken,
                response.RefreshToken
            );

            TempData["SuccessMessage"] = "Login successfully!";

            switch (user.Role)
            {
                case Role.Admin:
                    return RedirectToPage("/Index", new { area = "Admin" });

                case Role.Staff:
                    return RedirectToPage("/Index", new { area = "Staff" });

                case Role.Customer:
                    return RedirectToAction("Home", "Pages");
            }
            return View(loginRequest);
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
            ModelState.AddModelError("ErrorMessage", "Unexpected error: " + ex.Message);
            return View(loginRequest);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await authService.LogoutAsync(
            Guid.Parse(authenticatedUserService.UserId), cookieService!.GetRefreshToken()!
        );
        await cookieService.RemoveAuthCookiesAsync();
        TempData["SuccessMessage"] = "Logged out successfully!";
        return RedirectToAction(nameof(Login));
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
            return RedirectToAction(nameof(Login));
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
        var accessToken = Request.Cookies["Project_PRN222_G5.Web.AccessToken"];
        var refreshToken = Request.Cookies["Project_PRN222_G5.Web.RefreshToken"];
        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
        {
            logger.LogWarning("Missing tokens");
            return Unauthorized();
        }

        try
        {
            var principal = jwtService.GetClaimsPrincipalFromExpiredToken(accessToken);
            var userIdClaim = principal?.FindFirst("uid")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Invalid user ID");
                return Unauthorized();
            }

            var request = new RefreshTokenRequest
            {
                UserId = userId,
                RefreshToken = refreshToken
            };

            var response = await authService.RefreshTokenAsync(request);
            var currentUser = await authService.GetByIdAsync(userId);
            await cookieService.SetAuthCookiesAsync(currentUser, response.AccessToken, response.RefreshToken);

            return Json(new { AccessToken = response.AccessToken });
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(error.Key, string.Join(", ", error.Value));
            }

            return Unauthorized();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Refresh token failed");
            return Unauthorized();
        }
    }
}