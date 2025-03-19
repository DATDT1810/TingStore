// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using TingStore.Client.Areas.Admin.Services.Users;
using TingStore.Client.Areas.Admin.Services;
using TingStore.Client.Areas.User.Services.Products;
using TingStore.Client.Areas.Admin.Services.ProductManagement;
using TingStore.Client.Areas.User.Services.Cart;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Đăng ký IUserService
builder.Services.AddScoped<IUserService, UserService>();
// Đăng ký IProductService
builder.Services.AddScoped<IProductService, ProductService>();
// đăng ký DI cho CartService
builder.Services.AddScoped<ICartService, CartService>();


// admin/productmanagement
builder.Services.AddScoped<IProductManagementService, ProductManagementService>();



// Cấu hình HttpClient để gọi API Gateway
builder.Services.AddHttpClient("ApiGateway", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/"); // API Gateway
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
