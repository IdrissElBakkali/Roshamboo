using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Roshamboo.Core;
using Roshamboo.Services;
using Roshamboo.Services.Repository;
using Roshamboo.Store;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.EnableAnnotations();
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = $"[ Environment :{builder.Environment.EnvironmentName} ] Roshamboo API",
		Description = $"Build version :{typeof(Program).Assembly.GetName().Version}"
	});
});
builder.Services.AddControllers(options =>
{
	options.ModelMetadataDetailsProviders.Add(new NewtonsoftJsonValidationMetadataProvider());
}).AddNewtonsoftJson();

// Register Services
builder.Services.AddSingleton(new RoshambooGameContext());
builder.Services.AddScoped<IRoshambooGameRepository, RoshambooGameRepository>();
builder.Services.AddScoped<IComputerChoiceService, ComputerChoiceService>();
builder.Services.AddScoped<IShapeComparisonStrategyService, ShapeComparisonStrategyService>();
builder.Services.AddScoped<IRoshambooGameManagementService, RoshambooGameManagementService>();

var app = builder.Build();

// Configure Exception handler
app.UseExceptionHandler("/error");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
