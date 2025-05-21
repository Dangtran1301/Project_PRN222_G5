using Project_PRN222_G5.Application.DTOs.Cinema.Request;
using Project_PRN222_G5.Application.DTOs.Cinema.Response;
using Project_PRN222_G5.Domain.Entities.Cinema;

namespace Project_PRN222_G5.Application.Interfaces.Service.Identities;

public interface ICinemaService : IGenericService<Cinema, CreateCinemaDto, UpdateCinemaDto, CinemaResponse>;