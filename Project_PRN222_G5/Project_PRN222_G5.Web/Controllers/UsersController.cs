using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.DataAccess.DTOs.Users.Requests;
using Project_PRN222_G5.DataAccess.Exceptions;
using Project_PRN222_G5.DataAccess.Interfaces.Service;
using Project_PRN222_G5.Web.Views.Users;

namespace Project_PRN222_G5.Web.Controllers;

[Authorize]
public class UsersController(
    IUserService userService,
    IAuthenticatedUserService authenticatedUserService,
    ICookieService cookieService,
    IAuthService authService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Info()
    {
        var userId = Guid.Parse(authenticatedUserService.UserId);
        var userInfo = await userService.GetUserInfoById(userId);

        var viewModel = new InfoPageViewModel
        {
            User = userInfo,
            UpdateInfo = new UpdateInfoUser
            {
                Id = userInfo.Id,
                FullName = userInfo.FullName,
                PhoneNumber = userInfo.PhoneNumber,
                DayOfBirth = userInfo.DayOfBirth,
                Gender = userInfo.Gender,
            },
            ResetPassword = new ResetPasswordRequest()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile([Bind] UpdateInfoUser infoUser, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(authenticatedUserService.UserId);

        if (userId != infoUser.Id)
        {
            return Unauthorized();
        }

        await userService.UpdateAsync(userId, infoUser, cancellationToken);
        TempData["SuccessMessage"] = "Your profile was updated successfully!";
        return RedirectToAction(nameof(Info));
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(authenticatedUserService.UserId);

        try
        {
            if (await userService.ResetPassword(userId, request, cancellationToken))
            {
                TempData["SuccessMessage"] = "Password changed successfully. Please login again.";
                await authService.LogoutAsync(userId, cookieService.GetRefreshToken() ?? string.Empty, cancellationToken);
                await cookieService.RemoveAuthCookiesAsync();

                return RedirectToAction("Login", "Auth");
            }

            ModelState.AddModelError(string.Empty, "Password reset failed.");
        }
        catch (ValidationException ex)
        {
            ViewData["ActiveForm"] = "reset";
            if (ex.Errors is { Count: > 0 })
                foreach (var error in ex.Errors)
                {
                    foreach (var message in error.Value.Distinct())
                    {
                        ModelState.AddModelError(error.Key, message);
                    }
                }
            else
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        var userInfo = await userService.GetUserInfoById(userId, cancellationToken);
        var viewModel = new InfoPageViewModel
        {
            User = userInfo,
            UpdateInfo = new UpdateInfoUser
            {
                Id = userInfo.Id,
                FullName = userInfo.FullName,
                PhoneNumber = userInfo.PhoneNumber,
                DayOfBirth = userInfo.DayOfBirth,
                Gender = userInfo.Gender,
            },
            ResetPassword = request
        };

        return View(nameof(Info), viewModel);
    }
}