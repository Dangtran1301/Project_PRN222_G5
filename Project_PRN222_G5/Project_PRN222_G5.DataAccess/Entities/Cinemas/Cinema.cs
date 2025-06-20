﻿using Project_PRN222_G5.DataAccess.Entities.Common;

namespace Project_PRN222_G5.DataAccess.Entities.Cinemas;

public class Cinema : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ICollection<Room> Rooms { get; set; } = [];
}