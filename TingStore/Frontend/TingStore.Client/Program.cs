// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using TingStore.Client.Areas.Admin.Services.Users;
using TingStore.Client.Areas.Admin.Services;
using TingStore.Client.Areas.Admin.Services.Categories;
using TingStore.Client.Areas.User.Services.Products;
using TingStore.Client.Areas.User.Services.UserProfile;
using TingStore.Client.Areas.User.Services.Reviews;
using TingStore.Client.Areas.Admin.Services.ProductManagement;
using TingStore.Client.Areas.User.Services.Cart;
using TingStore.Client.Areas.User.Services.Categories;
using TingStore.Client.Areas.Admin.Services.OrderMangement;
using TingStore.Client.Areas.User.Services.JWT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Đăng ký IUserService
builder.Services.AddScoped<IUserService, UserService>();


// admin/categoryService
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Đăng ký IProductService
builder.Services.AddScoped<IProductService, ProductService>();

// Đăng ký IUserProfileService
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

// đăng ký DI cho CartService
builder.Services.AddScoped<ICartService, CartService>();

// Đăng ký ICategoryUserService
builder.Services.AddScoped<ICategoryUserService, CategoryUserService>();

// admin/productmanagement
builder.Services.AddScoped<IProductManagementService, ProductManagementService>();

builder.Services.AddScoped<IOrderManagementService, OrderManagementService>();

// Đăng ký IReviewProductService
builder.Services.AddScoped<IReviewProductService, ReviewProductService>();

// Đăng ký Jwt
builder.Services.AddScoped<JwtService>();

// Cấu hình HttpClient để gọi API Gateway
builder.Services.AddHttpClient("ApiGateway", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/"); // API Gateway
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromMinutes(2);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
