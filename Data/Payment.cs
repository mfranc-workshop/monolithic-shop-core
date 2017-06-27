using System;

namespace monolithic_shop_core.Data
{
    public enum PaymentStatus
    {
        None = 0, // change to Scheduled
        Processed = 1,
        Declined = 2
    }

    public class Card
    {
        public int Id { get; set; }
        public string Number { get; set; }
    }

    public class Buyer 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Buyer()
        {
        }

        public Buyer(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }

    public class Address 
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }

    public class Payment
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public Address Address { get; set; }
        public Guid OrderId { get; set; }
        public Card Card { get; set; }
        public PaymentType Type { get; set; }
        public PaymentStatus Status { get; set; }

        public Payment()
        {
            Status = PaymentStatus.None;
        }

        public Payment(Guid orderId)
            : this()
        {
            OrderId = orderId;
        }
    }
}