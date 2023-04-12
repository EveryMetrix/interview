using System.Collections.Generic;
using System.Threading.Tasks;
using Interview.AppService.Abstractions.Dtos;

namespace Interview.AppService.Abstractions
{
    public interface ICustomerAppService
    {
        Task<IEnumerable<CustomerDto>> GetCustomersByIdAsync(long customerId, int high, int low);
        Task<IEnumerable<CustomerDto>> GetCustomersByRankAsync(int start, int end);
        Task<IEnumerable<CustomerDto>> GetListAsync();
        Task<decimal> UpdateScoreAsync(long customerId, decimal scoreIncrement);
    }
}
