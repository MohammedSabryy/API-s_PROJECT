using Domain.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    internal class OrderConfigurations : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.OwnsOne(order => order.ShippingAddress, address => address.WithOwner());

            builder.HasMany(o => o.OrderItems).WithOne();

            builder.Property(order => order.PaymentStatus).HasConversion
                (
                            s => s.ToString(),
                            s => Enum.Parse<OrderPaymentStatus>(s)
                );

            builder.HasOne(order=>order.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);

            builder.Property(o=>o.SubTotal).HasColumnType("decimal(18,3)");
            
            
            



        }
    }
}
