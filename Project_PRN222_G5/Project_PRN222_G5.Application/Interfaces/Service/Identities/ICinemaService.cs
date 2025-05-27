using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.Infrastructure.Entities.Cinema;

namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;

public interface ICinemaService : IGenericService<Cinema, CreateCinemaDto, UpdateCinemaDto, CinemaResponse>;