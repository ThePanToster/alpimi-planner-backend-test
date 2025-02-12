﻿namespace AlpimiAPI.Entities.EClassroom.DTO
{
    public class UpdateClassroomDTO
    {
        public string? Name { get; set; }

        public int? Capacity { get; set; }

        public IEnumerable<Guid>? ClassroomTypeIds { get; set; }
    }
}
