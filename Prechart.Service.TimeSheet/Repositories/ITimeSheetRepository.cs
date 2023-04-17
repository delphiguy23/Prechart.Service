using Prechart.Service.Core.Service;
using Prechart.Service.TimeSheet.Models;

namespace Prechart.Service.TimeSheet.Repositories
{
    public interface ITimeSheetRepository :
         IHandler<TimeSheetRepository.TimeRecord, IResult<TimeSheetStatus>>,
         IHandler<TimeSheetRepository.GetTimeRecord, IResult<Database.Models.TimeSheet>>
    {
    }
}
