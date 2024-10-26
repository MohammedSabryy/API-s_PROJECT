using Domain.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class OrderWithIncludesSpecifications : Specifications<Order>
    {
        public OrderWithIncludesSpecifications(Guid id)
            :base(order=>order.Id==id)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.OrderItems);

        }

        public OrderWithIncludesSpecifications(string email)
            : base(order => order.UserEmail == email)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.OrderItems);

        }
    }
}
