global using VideoGamesAPI.Services;
global using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<ILogger>(svc => svc.GetRequiredService<ILogger<IGenreService>>());
builder.Services.AddTransient<IVideoGameService, VideoGameService>();
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddTransient<DbContext, DataContext>();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
