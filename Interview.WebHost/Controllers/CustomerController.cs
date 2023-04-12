using Interview.AppService.Abstractions;
using Interview.AppService.Abstractions.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Interview.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerAppService _customerAppService;

        public CustomerController(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerDto>> GetListAsync()
        {
            return await _customerAppService.GetListAsync();
        }

        [HttpPost("{customerId}/score/{score}")]
        public async Task<decimal> UpdateScopeAsync(long customerId, decimal score)
        {
            return await _customerAppService.UpdateScoreAsync(customerId, score);
        }
    }
}