using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.DataAccess.Entities.Identities.Users;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.BusinessLogic.Validation;

public class ValidationService(IUnitOfWork unitOfWork) : IValidationService
{
    public async Task<Dictionary<string, string[]>> ValidateAsync<T>(T model)
    {
        if (model == null)
        {
            return new Dictionary<string, string[]>
            {
                ["Model"] = ["Model cannot be null."]
            };
        }

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);

        try
        {
            Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
        }
        catch (Exception ex)
        {
            return new Dictionary<string, string[]>
            {
                ["ValidationError"] = [$"Validation failed: {ex.Message}"]
            };
        }

        var errors = validationResults
            .GroupBy(v => v.MemberNames.FirstOrDefault() ?? "General")
            .ToDictionary(
                g => g.Key,
                g => g.Select(v => v.ErrorMessage ?? "Invalid value").ToArray()
            );

        if (errors.Any())
        {
            throw new Exceptions.ValidationException(errors);
        }

        return errors;
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