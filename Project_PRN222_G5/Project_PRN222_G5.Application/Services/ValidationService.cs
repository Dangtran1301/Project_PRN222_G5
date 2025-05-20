using Project_PRN222_G5.Application.Interfaces.Validation;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.Application.Services;

public class ValidationService(IUnitOfWork unitOfWork) : IValidationService
{
    public async Task<Dictionary<string, string[]>> ValidateAsync<T>(T model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);
        Validator.TryValidateObject(model, validationContext, validationResults, true);

        var errors = validationResults
            .GroupBy(v => v.MemberNames.FirstOrDefault() ?? string.Empty)
            .ToDictionary(
                g => g.Key,
                g => g.Select(v => v.ErrorMessage).ToArray()
            );

        return (await Task.FromResult(errors))!;
    }

    public async Task ValidateUniqueUserAsync(string username, string email)
    {
        var userRepository = unitOfWork.Repository<User>();
        var existingUser = (await userRepository.FindAsync(u => u.Username == username)).FirstOrDefault()
                           ?? (await userRepository.FindAsync(u => u.Email == email)).FirstOrDefault();
        if (existingUser != null)
        {
            throw new ValidationException("Username or email already exists.");
        }
    }
}