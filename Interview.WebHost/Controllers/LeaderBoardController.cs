using Interview.AppService.Abstractions;
using Interview.AppService.Abstractions.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Interview.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeaderBoardController : ControllerBase
    {
        private readonly ICustomerAppService _customerAppService;

        public LeaderBoardController(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }

        [HttpGet("{customerId}")]
        public async Task<IEnumerable<CustomerDto>> GetCustomersByIdAsync(long customerId, int high, int low)
        {
            if (high < 0)
                throw new ArgumentException($"The value of parameter \"{nameof(high)}\" can not be less than 0.");
            if (low < 0)
                throw new ArgumentException($"The value of parameter \"{nameof(low)}\" can not be less than 0.");

            return await _customerAppService.GetCustomersByIdAsync(customerId, high, low);
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerDto>> GetCustomersByRankAsync(int start, int end)
        {
            if (start < 1)
                throw new ArgumentException($"The value of parameter \"{nameof(start)}\" can not be less than 1.");
            if (end < 1)
                throw new ArgumentException($"The value of parameter \"{nameof(end)}\" can not be less than 1.");
            if (start > end)
                throw new ArgumentException($"The value of parameter \"{nameof(end)}\" can not be less than \"{nameof(start)}\".");

            return await _customerAppService.GetCustomersByRankAsync(start, end);
        }
    }
}