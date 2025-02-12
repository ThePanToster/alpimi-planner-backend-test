﻿using AlpimiAPI.Entities.EClassroom;
using AlpimiAPI.Entities.EClassroom.DTO;
using AlpimiAPI.Entities.EClassroomType;
using AlpimiAPI.Entities.EClassroomType.DTO;
using AlpimiAPI.Entities.EDayOff;
using AlpimiAPI.Entities.EDayOff.DTO;
using AlpimiAPI.Entities.EGroup;
using AlpimiAPI.Entities.EGroup.DTO;
using AlpimiAPI.Entities.ELesson;
using AlpimiAPI.Entities.ELesson.DTO;
using AlpimiAPI.Entities.ELessonBlock;
using AlpimiAPI.Entities.ELessonPeriod;
using AlpimiAPI.Entities.ELessonPeriod.DTO;
using AlpimiAPI.Entities.ELessonType;
using AlpimiAPI.Entities.ELessonType.DTO;
using AlpimiAPI.Entities.ESchedule;
using AlpimiAPI.Entities.ESchedule.DTO;
using AlpimiAPI.Entities.EScheduleSettings;
using AlpimiAPI.Entities.EScheduleSettings.DTO;
using AlpimiAPI.Entities.EStudent;
using AlpimiAPI.Entities.EStudent.DTO;
using AlpimiAPI.Entities.ESubgroup;
using AlpimiAPI.Entities.ESubgroup.DTO;
using AlpimiAPI.Entities.ETeacher;
using AlpimiAPI.Entities.ETeacher.DTO;

namespace AlpimiAPI.Utilities
{
    public static class DataTrimmer
    {
        public static DayOffDTO Trim(DayOff data)
        {
            return new DayOffDTO
            {
                Id = data.Id,
                Name = data.Name,
                From = data.From,
                To = data.To
            };
        }

        public static ScheduleDTO Trim(Schedule data)
        {
            return new ScheduleDTO { Id = data.Id, Name = data.Name };
        }

        public static LessonPeriodDTO Trim(LessonPeriod data)
        {
            return new LessonPeriodDTO { Id = data.Id, Start = data.Start, };
        }

        public static ClassroomTypeDTO Trim(ClassroomType data)
        {
            return new ClassroomTypeDTO { Id = data.Id, Name = data.Name };
        }

        public static ClassroomDTO Trim(Classroom data)
        {
            return new ClassroomDTO
            {
                Id = data.Id,
                Name = data.Name,
                Capacity = data.Capacity
            };
        }

        public static LessonTypeDTO Trim(LessonType data)
        {
            return new LessonTypeDTO
            {
                Id = data.Id,
                Name = data.Name,
                Color = data.Color
            };
        }

        public static GroupDTO Trim(Group data)
        {
            return new GroupDTO
            {
                Id = data.Id,
                Name = data.Name,
                StudentCount = data.StudentCount
            };
        }

        public static TeacherDTO Trim(Teacher data)
        {
            return new TeacherDTO
            {
                Id = data.Id,
                Name = data.Name,
                Surname = data.Surname
            };
        }

        public static SubgroupDTO Trim(Subgroup data)
        {
            return new SubgroupDTO
            {
                Id = data.Id,
                Name = data.Name,
                StudentCount = data.StudentCount,
                Group = Trim(data.Group)
            };
        }

        public static StudentDTO Trim(Student data)
        {
            return new StudentDTO
            {
                Id = data.Id,
                AlbumNumber = data.AlbumNumber,
                Group = Trim(data.Group)
            };
        }

        public static LessonDTO Trim(Lesson data)
        {
            return new LessonDTO
            {
                Id = data.Id,
                Name = data.Name,
                CurrentHours = data.CurrentHours,
                AmountOfHours = data.AmountOfHours,
                LessonType = Trim(data.LessonType),
                Subgroup = Trim(data.Subgroup)
            };
        }

        public static ScheduleSettingsDTO Trim(ScheduleSettings data)
        {
            return new ScheduleSettingsDTO
            {
                Id = data.Id,
                SchoolHour = data.SchoolHour,
                SchoolYearStart = data.SchoolYearStart,
                SchoolYearEnd = data.SchoolYearEnd,
                SchoolDays = data.SchoolDays
            };
        }

        public static LessonBlockDTO Trim(LessonBlock data)
        {
            return new LessonBlockDTO
            {
                Id = data.Id,
                LessonDate = data.LessonDate,
                LessonStart = data.LessonStart,
                LessonEnd = data.LessonEnd,
                Lesson = Trim(data.Lesson),
                Classroom = data.Classroom == null ? null : Trim(data.Classroom),
                Teacher = data.Teacher == null ? null : Trim(data.Teacher),
                ClusterId = data.ClusterId
            };
        }
    }
}
