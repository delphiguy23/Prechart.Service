using Prechart.Service.Core.Service;
using Prechart.Service.TimeSheet.Models;

namespace Prechart.Service.TimeSheet.Service;

public interface ITimeSheetService:
    IHandler<TimeSheetService.InsertTimeRecord, IResult<TimeSheetStatus>>
{
}