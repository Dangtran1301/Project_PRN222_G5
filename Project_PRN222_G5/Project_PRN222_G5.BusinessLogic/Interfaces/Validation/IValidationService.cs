namespace Project_PRN222_G5.BusinessLogic.Interfaces.Validation;

public interface IValidationService
{
    Dictionary<string, string[]> Validate<T>(T model);

    Task ValidateUniqueUserAsync(string username, string email);
    Task ValidateUniqueCinemaAsync(string name, Guid? excludingId = null);

}