namespace Project_PRN222_G5.BusinessLogic.Interfaces.Validation;

public interface IValidationService
{
    bool TryValidate<T>(T model, out Dictionary<string, string[]> errors);

    Task ValidateUniqueUserAsync(string username, string email);

    Task ValidateUniqueCinemaAsync(string name, Guid? excludingId = null);

    Task ValidateCinemaCanBeDeletedAsync(Guid cinemaId);
}