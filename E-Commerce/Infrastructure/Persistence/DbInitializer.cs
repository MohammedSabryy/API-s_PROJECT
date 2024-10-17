global using Domain.Contracts;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using Persistence.Data;
global using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreContext _storeContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public DbInitializer(StoreContext storeContext, RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _storeContext = storeContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitilaizeAsync()
        {
            try
            {
                if (_storeContext.Database.GetPendingMigrations().Any())
                    await _storeContext.Database.MigrateAsync();


                if (!_storeContext.ProductTypes.Any())
                {
                    var TypesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");

                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                    if (Types != null && Types.Any())
                    {
                        await _storeContext.ProductTypes.AddRangeAsync(Types);
                        await _storeContext.SaveChangesAsync();
                    }
                }

                if (!_storeContext.ProductBrands.Any())
                {
                    var BrandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");

                    var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                    if (Brands != null && Brands.Any())
                    {
                        await _storeContext.ProductBrands.AddRangeAsync(Brands);
                        await _storeContext.SaveChangesAsync();
                    }
                }

                if (!_storeContext.Products.Any())
                {
                    var ProductsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                    if (Products != null && Products.Any())
                    {
                        await _storeContext.Products.AddRangeAsync(Products);
                        await _storeContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception) 
            {
                throw;
            }
        }

        public async Task InitilaizeIdentityAsync()
        {
            if(!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!_userManager.Users.Any())
            {
                var superAdminUser = new User
                {
                    DisplayName = "Super Admin User",
                    Email = "superAdminUser@Gmail.com",
                    UserName = "superAdminUser",
                    PhoneNumber = "01121054562",
                };
                var adminUser = new User
                {
                    DisplayName = "Admin User",
                    Email = "adminUser@Gmail.com",
                    UserName = "adminUser",
                    PhoneNumber = "01121054562",
                };

                await _userManager.CreateAsync(superAdminUser,"Passw0rd"); 
                await _userManager.CreateAsync(adminUser,"Passw0rd");

                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");

            }
        }
    }
}
//D:\.net course\API\E-Commerce\Infrastructure\Persistence\Data\Seeding\types.json