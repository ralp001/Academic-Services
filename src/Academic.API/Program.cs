using Academic.API.Middleware;
using Academic.Application;
using Academic.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// This method registers the DbContext, Repositories, and the Logger Adapter
builder.Services.AddInfrastructureServices(builder.Configuration);

// This method registers MediatR, FluentValidation (Behaviors), and IAppLogger (Abstraction)
builder.Services.AddApplicationServices();


// Registers controllers (our ClassesController and SubjectsController)
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();           

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// --- 3. HTTP Request Pipeline Configuration ---

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // These two lines enable the Swagger UI at /swagger
    app.UseSwagger();
    app.UseSwaggerUI();

    
}

// Global Exception Handling Middleware (Recommended for production, not included here)

app.UseHttpsRedirection();

// UseRouting is often implicit but good to keep if needed
app.UseAuthorization();

// Maps the attribute-routed controllers (like ClassesController)
app.MapControllers();

app.Run();