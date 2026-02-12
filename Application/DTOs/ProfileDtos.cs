using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.User;

namespace Application.DTOs
{
    public record ProfileDto(string Email, string Name, string Phone, User.TypeUser Role);

    public record ProfileMeDto(
        string Email,
        string Name,
        string Phone,
        User.TypeUser Role,
        string UserImg,
        StudentSummary? Student,
        TrainerSummary? Trainer
    );

    public record StudentSummary(int ActiveSubscriptionCount);

    public record TrainerSummary(Guid TrainerId, string Specialization, int SchedulesCount);

    public record SubscriptionDto(
        Guid SubId,
        DateTime StartDate,
        DateTime EndDate,
        string Status,
        TariffDto Tariff,
        DiscountDto? Discount,
        IReadOnlyList<VisitDto> Visits
    );

    public record VisitDto(Guid VisitId, DateTime ActualDate, ScheduleDto Schedule);

    public record ScheduleDto(
        Guid SheduleId,
        string DayOfWeek,
        TimeOnly StartTime,
        string Room,
        int Status,
        string DanceTypeName,
        Guid DanceTypeId,
        string TrainerName,
        Guid TrainerId
    );


    public record MyScheduleItemDto(
        Guid? VisitId,
        Guid SheduleId,
        string DayOfWeek,
        TimeOnly StartTime,
        string Room,
        int Status,
        string DanceTypeName,
        Guid DanceTypeId,
        string TrainerName,
        Guid TrainerId
    );

    public record TariffDto(Guid TariffId, string Name, decimal Price, int DaysValid);

    public record DiscountDto(Guid DiscountId, string Name, int Percent);

    public record BookingRequest(Guid SheduleId, DateTime ActualDate);
    public record RescheduleRequest(Guid VisitId, Guid NewSheduleId, DateTime NewDate);
}
