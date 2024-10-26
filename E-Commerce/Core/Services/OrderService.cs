using Domain.Entities.OrderEntities;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using OrderAddress = Domain.Entities.OrderEntities.Address; 
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Services
{
    internal class OrderService (IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository)
        : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest request, string userEmail)
        {
            var address = mapper.Map<OrderAddress>(request.ShippingAddress);
            
            var basket = await basketRepository.GetBasketAsync(request.BasketId)??throw new BasketNotFoundException(request.BasketId);

            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>()
                    .GetAsync(item.Id) 
                    ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(item, product));
            }

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(request.DeliveryMethodId) ?? throw new DeliveryMethodNotFoundException(request.DeliveryMethodId);

            var subTotal = orderItems.Sum(item=>item.Quantity*item.Price);

            var order = new Order(userEmail, address, deliveryMethod, orderItems, subTotal);

            await unitOfWork.GetRepository<Order,Guid>().AddAsync(order);

            await unitOfWork.SaveChangesAsync();

            return mapper.Map<OrderResult>(order);
        }

        private OrderItem CreateOrderItem(BasketItem item, Product product) => new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl)
            , item.Quantity, product.price);

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var order = await unitOfWork.GetRepository<Order,Guid>().GetAsync(new OrderWithIncludesSpecifications(id))?? throw new OrderNotFoundException(id);
            return mapper.Map<OrderResult>(order);

        }

        public async Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string email)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderWithIncludesSpecifications(email));

            return mapper.Map<IEnumerable<OrderResult>>(orders);
        }

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var method = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodResult>>(method);
        }
    }
}
