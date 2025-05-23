﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Responses;
using Project_PRN222_G5.Application.Interfaces;

namespace Project_PRN222_G5.Web.Pages.Users
{
    public class DetailsModel(IUserService userService) : PageModel
    {
        public new UserResponse User { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                User = await userService.GetByIdAsync(id);
                return Page();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}