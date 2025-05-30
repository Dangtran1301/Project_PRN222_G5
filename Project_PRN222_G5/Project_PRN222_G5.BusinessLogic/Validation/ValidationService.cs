using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.DataAccess.Entities.Cinemas;
using Project_PRN222_G5.DataAccess.Entities.Users;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.BusinessLogic.Validation;

public class ValidationService(IUnitOfWork unitOfWork) : IValidationService
{
    public Dictionary<string, string[]> Validate<T>(T model)
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

        Validator.TryValidateObject(model, validationContext, validationResults, true);

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
        bool exists = await userRepository.AnyAsync(u => u.Username == username || u.Email == email);
        if (exists)
        {
            throw new ValidationException("Username or email already exists.");
        }
    }

    public async Task ValidateUniqueCinemaAsync(string name, Guid? excludingId = null)
    {
        var repo = unitOfWork.Repository<Cinema>();

        var exists = excludingId.HasValue
            ? await repo.AnyAsync(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && c.Id != excludingId.Value)
            : await repo.AnyAsync(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            throw new ValidationException($"Cinema with the name '{name}' already exists.");
        }
    }
}