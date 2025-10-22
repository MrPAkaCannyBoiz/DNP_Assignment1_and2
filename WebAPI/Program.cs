using FileRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
//if you want to use Swagger, you need to add the following line
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserFileRepository>();
builder.Services.AddScoped<IPostRepository, PostFileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// Add CORS services

// Configure CORS with environment-aware policy and configuration-driven allowed origins
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // Development: allow common local dev origins (be explicit)
            policy.WithOrigins("http://localhost:3000", "http://localhost:5122", "https://localhost:7093")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            // .AllowCredentials() // enable only if you use cookies and you list exact origins
        }
        else
        {
            // Production: only allow configured origins (do NOT AllowAnyOrigin here)
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            // Use .AllowCredentials() only when required and only with explicit origins
        }
    });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    // use swagger only in development mode
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Enforce HTTPS in non-development environments
    app.UseHttpsRedirection();
}

    app.UseCors("DefaultPolicy");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
