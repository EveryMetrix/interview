using System.Collections.Generic;

namespace Interview.Domain.Customers
{
    public class Customer
    {
        public Customer(long id)
        {
            Id = id;
        }

        public Customer(long id, decimal score) : this(id)
        {
            Score = score;
        }

        public long Id { get; protected set; }
        public decimal Score { get; protected set; } = decimal.Zero;

        public void AddScore(decimal scoreIncrement) => Score += scoreIncrement;
    }

    public class CustomerComparer : IComparer<Customer>
    {
        public int Compare(Customer x, Customer y)
        {
            return x.Score == y.Score
                ? x.Id.CompareTo(y.Id)
                : -x.Score.CompareTo(y.Score);
        }
    }
}
