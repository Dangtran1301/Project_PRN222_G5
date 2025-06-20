﻿using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;

namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;

public interface ICinemaService : IGenericService<DataAccess.Entities.Cinemas.Cinema, CreateCinemaDto, UpdateCinemaDto, CinemaResponse>;