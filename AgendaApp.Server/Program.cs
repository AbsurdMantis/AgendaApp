using AgendaApp.Server.DTOs;
using AgendaApp.Server.Repositories;
using AgendaApp.Server.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ContatoValidator>();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
builder.Services.AddScoped<IContatoService, ContatoService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
