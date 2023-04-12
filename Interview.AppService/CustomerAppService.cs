using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interview.AppService.Abstractions;
using Interview.AppService.Abstractions.Dtos;
using Interview.Domain.Customers;

namespace Interview.AppService
{
    public class CustomerAppService : ICustomerAppService
    {
        private static readonly ConcurrentDictionary<long, decimal> _CustomerIdScoreMap = new ConcurrentDictionary<long, decimal>();
        private static readonly SortedSet<Customer> _Customers = new SortedSet<Customer>(new CustomerComparer());

        public async Task<IEnumerable<CustomerDto>> GetCustomersByIdAsync(long customerId, int high, int low)
        {
            if (!_CustomerIdScoreMap.TryGetValue(customerId, out var score))
                throw new KeyNotFoundException($"No customer with ID({customerId}) was found.");

            if (_Customers.Count == 0)
                return Enumerable.Empty<CustomerDto>();

            var customers = new Customer[_Customers.Count];
            _Customers.CopyTo(customers);
            var rank = 0;
            var customer = customers.FirstOrDefault(x =>
            {
                rank++;
                return x.Id == customerId;
            });
            if (customer == null)
                return Enumerable.Empty<CustomerDto>();

            var start = Math.Max(rank - high, 1);
            var end = rank + low;

            var records = customers.Skip(start - 1).Take(end - start + 1)
                .Select(x => new CustomerDto
                {
                    CustomerId = x.Id,
                    Score = x.Score,
                    Rank = start++
                });

            return await Task.FromResult(records);
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersByRankAsync(int start, int end)
        {
            if (_Customers.Count == 0)
                return Enumerable.Empty<CustomerDto>();

            var customers = new Customer[_Customers.Count];
            _Customers.CopyTo(customers);

            var records = customers.Skip(start - 1).Take(end - start + 1)
                .Select(x => new CustomerDto
                {
                    CustomerId = x.Id,
                    Score = x.Score,
                    Rank = start++
                });

            return await Task.FromResult(records);
        }

        public async Task<IEnumerable<CustomerDto>> GetListAsync()
        {
            if (_Customers.Count == 0)
                return Enumerable.Empty<CustomerDto>();

            var customers = new Customer[_Customers.Count];
            _Customers.CopyTo(customers);

            var start = 0;
            var records = customers
                .Select(x => new CustomerDto
                {
                    CustomerId = x.Id,
                    Score = x.Score,
                    Rank = ++start
                });

            return await Task.FromResult(records);
        }

        public async Task<decimal> UpdateScoreAsync(long customerId, decimal scoreIncrement)
        {
            var score = _CustomerIdScoreMap.AddOrUpdate(customerId, scoreIncrement, (_, oldScore) =>
            {
                var newScore = oldScore + scoreIncrement;
                if (oldScore > 0)
                    _Customers.RemoveWhere(x => x.Id == customerId);
                return newScore;
            });

            if (score > 0)
                _Customers.Add(new Customer(customerId, score));

            return await Task.FromResult(score);
        }
    }
}
