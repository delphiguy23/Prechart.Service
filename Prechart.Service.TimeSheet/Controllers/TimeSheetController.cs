using Microsoft.AspNetCore.Mvc;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.Service;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Prechart.Service.Globals.Events.Person;
using Prechart.Service.TimeSheet.Service;

namespace Prechart.Service.TimeSheet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/platform/service/api/Person/timesheet")]
    public class TimeSheetController : ControllerBase
    {
        private readonly ITimeSheetService _service;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly IRequestClient<IGetPerson> _Person;

        public TimeSheetController(ITimeSheetService service, IPublishEndpoint publishEndpoint, IMapper mapper,
            IRequestClient<IGetPerson> Person)
        {
            _service = service;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _Person = Person;
        }

        [HttpPost]
        public async Task<ActionResult> TimeRecordEntry([FromBody] Models.TimeRecord timerecord)
        {
            var result = await _service.Handle(new TimeSheetService.InsertTimeRecord
            {
                Id = timerecord.Id,
                RecordDate = timerecord.RecordDate,
                RecordType = (TimeRecordType) timerecord.RecordType,
            });

            return result.ToActionResult();
        }
        
        [HttpGet]
        [Route("{Personid}")]
        public async Task<ActionResult> GetPersonDetails(int Personid)
        {
            var Person = await _Person.GetResponse<IPerson>(new {PersonId = Personid});

            if ( (Person?.Message?.Id ?? 0) == 0)
            {
                return NotFound();
            }
            
            return Person.Some().ToActionResult();
        }
    }
}