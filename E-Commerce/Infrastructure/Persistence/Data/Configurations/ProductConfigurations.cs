global using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(Product=>Product.ProductBrand)
                .WithMany()
                .HasForeignKey(Product=>Product.BrandId);
            
            builder.HasOne(Product => Product.ProductType)
                .WithMany()
                .HasForeignKey(Product => Product.TypeId);

            builder.Property(product=>product.price).HasColumnType("Decimal(18,3)");
        }
    }
}
