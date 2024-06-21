using Hangfire;
using HangFireIntegration;
using HangFireIntegration.ActionFilters;
using HangFireIntegration.Data;
using HangFireIntegration.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dashboard = builder.Configuration.GetSection("HangfireConfig:EndPoint").Value;
var title = builder.Configuration.GetSection("HangfireConfig:Name").Value;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configurations 
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Hangfire Configurations
builder.Services.AddHangfire(x =>
    x.UseRecommendedSerializerSettings().UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

// Email Configurations
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireServer();

app.UseHangfireDashboard(dashboard, new DashboardOptions
{
    DashboardTitle = title,
    Authorization = [new HangfireAuthorizationFilter()]
});

app.Run();
