using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.DTOs
{
    public record SheduleDto(Guid SheduleId, string DayOfWeek, TimeOnly StartTime, string Room, int Status);

    public record CreateSheduleDtos(Guid SheduleId, Guid DanceId, Guid TrainerId, string DayOfWeek, TimeOnly StartTime, string Room, int Status);
    public record DanceDto(Guid DanceId, string Name, string Category, string DanceImg);

    public record TrainerDto(Guid TrainerId, string Specialization, string Name, string UserImg);
}
