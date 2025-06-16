using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.DataAccess.Entities.Cinemas;
using Project_PRN222_G5.DataAccess.Entities.Users;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using System.ComponentModel.DataAnnotations;
using ValidationException = Project_PRN222_G5.DataAccess.Exceptions.ValidationException;

namespace Project_PRN222_G5.BusinessLogic.Validation;

public class ValidationService(IUnitOfWork unitOfWork) : IValidationService
{
    public bool TryValidate<T>(T model, out Dictionary<string, string[]> errors)
    {
        errors = new Dictionary<string, string[]>();

        if (model == null)
        {
            errors["Model"] = new[] { "Model cannot be null." };
            return false;
        }

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);

        bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        if (!isValid)
        {
            errors = validationResults
                .GroupBy(v => v.MemberNames.FirstOrDefault() ?? "General")
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(v => v.ErrorMessage ?? "Invalid value").ToArray()
                );
        }

        return isValid;
    }

    public async Task ValidateUniqueUserAsync(string username, string email)
    {
        var userRepository = unitOfWork.Repository<User>();
        var exists = await userRepository.AnyAsync(u => u.Username == username || u.Email == email);
        if (exists)
        {
            throw new ValidationException("Username or email already exists.");
        }
    }

    public async Task ValidateUniqueCinemaAsync(string name, Guid? excludingId = null)
    {
        var repo = unitOfWork.Repository<Cinema>();

        var exists = excludingId.HasValue
            ? await repo.AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != excludingId.Value)
            : await repo.AnyAsync(c => c.Name.ToLower() == name.ToLower());

        if (exists)
        {
            throw new ValidationException($"Cinema with the name '{name}' already exists.");
        }
    }

    public async Task ValidateCinemaCanBeDeletedAsync(Guid cinemaId)
    {
        var cinema = await unitOfWork.Repository<Cinema>().GetByIdAsync(cinemaId);
        if (cinema == null)
            throw new ValidationException("Cinema not found.");

        var hasRooms = (await unitOfWork.Repository<Room>().FindAsync(r => r.CinemaId == cinemaId)).Any();
        if (hasRooms)
            throw new ValidationException("Cannot delete cinema because it has associated rooms.");
    }
}