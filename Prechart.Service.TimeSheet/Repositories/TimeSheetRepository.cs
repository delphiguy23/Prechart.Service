using System.Linq;
using Microsoft.EntityFrameworkCore;
using Prechart.Service.Core.Service;
using System.Threading.Tasks;
using Prechart.Service.TimeSheet.Database.Context;
using Prechart.Service.TimeSheet.Models;

namespace Prechart.Service.TimeSheet.Repositories
{
    public partial class TimeSheetRepository : ITimeSheetRepository
    {
        private readonly ITimeSheetContext _context;

        public TimeSheetRepository(ITimeSheetContext context)
        {
            _context = context;
        }

        public async Task<IResult<Database.Models.TimeSheet>> Handle(GetTimeRecord request)
        {
            var result = await _context.TimeSheet.FirstOrDefaultAsync(t =>
                t.PersonId == request.Id &&
                t.Date == request.RecordDate);

            return result.Some();
        }

        public async Task<IResult<TimeSheetStatus>> Handle(TimeRecord request)
        {
            var tsStatus = TimeSheetStatus.None;

            var a = await _context.TimeSheet.ToListAsync();

            var timesheet = await _context.TimeSheet
                .Where(ts => ts.PersonId == request.Id)
                .Where(ts => ts.Date == System.DateTime.Now.Date)
                .FirstOrDefaultAsync();

            if (timesheet is null && request.RecordType == Core.Models.TimeRecordType.TimeIn)
            {
                await _context.TimeSheet.AddAsync(new Database.Models.TimeSheet
                {
                    PersonId = request.Id,
                    Date = System.DateTime.Now.Date,
                    TimeIn = System.DateTime.Now,
                });

                tsStatus = TimeSheetStatus.TimeInOk;
            }

            if (timesheet is not null && request.RecordType != Core.Models.TimeRecordType.TimeIn)
            {
                switch (request.RecordType)
                {
                    case Core.Models.TimeRecordType.LunchIn:
                        timesheet.LunchIn = System.DateTime.Now;
                        tsStatus = TimeSheetStatus.LunchInOk;
                        break;
                    case Core.Models.TimeRecordType.LunchOut:
                        timesheet.LunchOut = System.DateTime.Now;
                        tsStatus = TimeSheetStatus.LunchOutOk;
                        break;
                    case Core.Models.TimeRecordType.TimeOut:
                        timesheet.TimeOut = System.DateTime.Now;
                        tsStatus = TimeSheetStatus.TimeOutOk;
                        break;
                }
            }

            if (new[]
                {
                    TimeSheetStatus.TimeInOk, TimeSheetStatus.LunchOutOk, TimeSheetStatus.LunchInOk,
                    TimeSheetStatus.TimeOutOk
                }.Contains(tsStatus))
            {
                await _context.SaveChangesAsync();
            }

            return tsStatus.Some();
        }
    }
}