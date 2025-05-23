﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Domain.Entities.Users.Enum;

namespace Project_PRN222_G5.Web.Pages.Users
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class CreateModel(IUserService userService) : PageModel
    {
        [BindProperty]
        public RegisterUserRequest Input { get; set; } = new();

        public IActionResult OnGet()
        {
            ViewData["Roles"] = new List<SelectListItem>
            {
                new() { Value = nameof(Role.Customer), Text = nameof(Role.Customer) },
                new() { Value = nameof(Role.Staff), Text = nameof(Role.Staff) }
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await userService.CreateAsync(Input);
                return RedirectToPage(PageRoutes.UsersIndex);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}