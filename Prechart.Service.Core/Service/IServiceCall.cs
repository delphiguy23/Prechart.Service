using System.Net.Http;
using System.Threading.Tasks;

namespace Prechart.Service.Core.Service
{
    public interface IServiceCall
    {
        Task<IResult<HttpContent>> Send(ServiceCallRequest request);
    }
}
