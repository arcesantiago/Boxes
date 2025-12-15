using Boxes.API.Middleware;
using Boxes.Application;
using Boxes.Infrastructure;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Boxes API",
        Version = "v1",
        Description = "API para gesti�n de turnos y talleres",
        Contact = new OpenApiContact
        {
            Name = "Boxes Team",
            Email = "support@boxes.com"
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddAplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Boxes API v1");
        c.RoutePrefix = string.Empty;
        c.DisplayRequestDuration();
    });
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();