namespace Prechart.Service.TimeSheet.Models;

public enum TimeSheetStatus
{
    None,
    TimeInOk,
    LunchOutOk,
    LunchInOk,
    TimeOutOk,
    TimeInAlreadyExists,
    TimeOutWithOutTimeIn,
    TimeOutWithOutLunchIn,
    LunchOutWithOutTimeIn,
    LunchInWithOutLunchOut,
    PersonNotFound,
}