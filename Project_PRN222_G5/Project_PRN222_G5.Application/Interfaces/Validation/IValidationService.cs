namespace Project_PRN222_G5.BusinessLogic.Interfaces.Validation;

public interface IValidationService
{
    Task<Dictionary<string, string[]>> ValidateAsync<T>(T model);

    Task ValidateUniqueUserAsync(string username, string email);
}