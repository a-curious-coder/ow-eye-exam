using CanYouC_.Controllers;
using CanYouC_.Interfaces;
using CanYouC_.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication("basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("basic", null);
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "BasicAuth", Version = "v1" });
    o.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header."
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "basic"
                    }
                },
                new string[] {}
        }
    });
});

// Add services to the container.
builder.Services.AddScoped<IRawScheduleController, RawScheduleController>();
builder.Services.AddScoped<INoticeLeaseController, NoticeLeaseController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/schedules", [Authorize] ([FromServices] IRawScheduleController rawScheduleController) => {
    return rawScheduleController.GetRawSchedules();
});

app.MapGet("/GetNoticeLeaseSchedules", [Authorize] ([FromServices] INoticeLeaseController noticeLeaseController) => {
    return noticeLeaseController.GetNoticeLeases();
});

app.MapGet("/GetNoticeLeaseSchedulesFromRawScheduleData", [Authorize] ([FromServices] INoticeLeaseController noticeLeaseController) => {
    return noticeLeaseController.GetNoticeLeaseFromRawSchedules();
});

app.Run();
