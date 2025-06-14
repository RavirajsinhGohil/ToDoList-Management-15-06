using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDoListManagement.Entity.Data;
using ToDoListManagement.Repository.Implementations;
using ToDoListManagement.Repository.Interfaces;
using ToDoListManagement.Service.Implementations;
using ToDoListManagement.Service.Interfaces;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ToDoListDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ToDoListDbConnection"));
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("Token"))
                {
                    context.Token = context.Request.Cookies["Token"];
                }
                return Task.CompletedTask;  
            },
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.Redirect("/Auth/Login");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddHttpContextAccessor();

WebApplication? app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    IServiceProvider? services = scope.ServiceProvider; var context = services.GetRequiredService<ToDoListDbContext>(); context.Database.Migrate();
}

app.UseExceptionHandler("/ErrorPages/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/ErrorPages/ShowError/{0}");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
