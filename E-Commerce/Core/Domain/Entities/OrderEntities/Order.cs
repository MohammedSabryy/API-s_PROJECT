using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderEntities
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, Address shippingAddress,  DeliveryMethod deliverMethod,  ICollection<OrderItem> orderItems, decimal subTotal)
        {
            
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliverMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
        }

        public string UserEmail{ get; set; }
        public Address ShippingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;
        public DeliveryMethod DeliveryMethod { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal SubTotal { get; set; }
        public string paymentIntentId{ get; set; } = string.Empty;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

    }
}
