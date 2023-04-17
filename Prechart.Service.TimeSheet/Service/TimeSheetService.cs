using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Events.Person;
using Prechart.Service.TimeSheet.Models;
using Prechart.Service.TimeSheet.Repositories;

namespace Prechart.Service.TimeSheet.Service;

public partial class TimeSheetService : ITimeSheetService
{
    private readonly ITimeSheetRepository _repository;
    private readonly ILogger<TimeSheetService> _logger;
    private readonly IRequestClient<IGetPerson> _Person;

    public TimeSheetService(ITimeSheetRepository repository, ILogger<TimeSheetService> logger,
        IRequestClient<IGetPerson> Person)
    {
        _repository = repository;
        _logger = logger;
        _Person = Person;
    }

    public async Task<IResult<TimeSheetStatus>> Handle(InsertTimeRecord request)
    {
        var Person = await _Person.GetResponse<IPerson>(new {Id = request.Id});
        var status = TimeSheetStatus.TimeInOk;

        if (Person is not null && (Person?.Message?.Id ?? 0) > 0)
        {
            var timeRecord = await _repository.Handle(new TimeSheetRepository.GetTimeRecord
            {
                Id = request.Id,
                RecordDate = request.RecordDate,
            }) as IsSome<Database.Models.TimeSheet>;

            if (timeRecord is not null && request.RecordType == Core.Models.TimeRecordType.TimeIn)
            {
                return TimeSheetStatus.TimeInAlreadyExists.Some();
            }

            if (timeRecord is not null)
            {
                if (request.RecordType == Core.Models.TimeRecordType.LunchOut)
                {
                    if (timeRecord.Value.TimeIn != DateTime.Now.Date)
                    {
                        return TimeSheetStatus.LunchOutWithOutTimeIn.Some();
                    }

                    status = TimeSheetStatus.LunchOutOk;
                }

                if (request.RecordType == Core.Models.TimeRecordType.LunchIn)
                {
                    if (timeRecord.Value.LunchOut != DateTime.Now.Date)
                    {
                        return TimeSheetStatus.LunchInWithOutLunchOut.Some();
                    }

                    status = TimeSheetStatus.LunchInOk;
                }

                if (request.RecordType == Core.Models.TimeRecordType.TimeOut)
                {
                    if (timeRecord.Value.TimeIn != DateTime.Now.Date)
                    {
                        return TimeSheetStatus.TimeOutWithOutTimeIn.Some();
                    }

                    if (timeRecord.Value.LunchIn != DateTime.Now.Date)
                    {
                        return TimeSheetStatus.TimeOutWithOutLunchIn.Some();
                    }

                    status = TimeSheetStatus.LunchOutOk;
                }
            }

            if (new[]
                {
                    TimeSheetStatus.TimeInOk, 
                    TimeSheetStatus.LunchOutOk, 
                    TimeSheetStatus.LunchInOk,
                    TimeSheetStatus.TimeOutOk
                }.Contains(status))
            {
                var result = await _repository.Handle(new TimeSheetRepository.TimeRecord
                {
                    Id = request.Id,
                    RecordDate = request.RecordDate,
                    RecordType = request.RecordType,
                });

                return result;
            }


            return status.Some();
        }

        return TimeSheetStatus.PersonNotFound.Some();
    }
}