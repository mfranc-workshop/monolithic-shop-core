using System;
using System.Collections.Generic;
using System.Linq;

namespace monolithic_shop_core.Data
{
    public enum OrderStatus
    {
        Delivering = 0,
        WaitingForWarehouse,
        WaitingForPayment,
        Blocked,
        Finished
    }

    public class Order
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public Buyer Buyer { get; set; }
        public Payment Payment { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }

        public Order()
        {
            Id = Guid.NewGuid();
        }

        public Order(ICollection<ProductOrder> productOrders)
            : this()
        {
            ProductOrders = productOrders;
        }

        public decimal Price
        {
            get
            {
                return ProductOrders.Sum(productOrder => productOrder.Count*productOrder.Product.Price);
            }
        }

        public void PayByTransfer(Payment payment)
        {
            payment.Price = this.Price;
            this.Payment = payment;
            this.Status = OrderStatus.WaitingForPayment;
        }

        public void PayByCard(Payment payment, bool successfullPayment)
        {
            payment.Price = this.Price;
            this.Payment = payment;
            this.Status = successfullPayment ? OrderStatus.WaitingForWarehouse : OrderStatus.Blocked;

        }

        public void AddBuyer(Buyer buyer)
        {
            this.Buyer = buyer;
        }

        public void Delivered()
        {
            this.Status = OrderStatus.Finished;
        }

        public void Delivering()
        {
            this.Status = OrderStatus.Delivering;
        }

        public void TransferReceived()
        {
            this.Status = OrderStatus.WaitingForWarehouse;
        }
    }
}