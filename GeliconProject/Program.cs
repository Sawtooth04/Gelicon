using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using GeliconProject.Middlewares;
using GeliconProject.Utils.JWTValidationParameters;
using GeliconProject.Hubs.Room;
using GeliconProject.Storage.Abstractions.Context;
using GeliconProject.Storage.Gelicon.Context;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Gelicon;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomObserversProvider;
using GeliconProject.Hubs.Room.Realizations.Threads.RoomObserversProvider;
using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsController;
using GeliconProject.Hubs.Room.Realizations.Threads.ThreadsController;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddJWTValidationParameters();
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
});
builder.Services.AddCors();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(new JWTValidationParameters().SetJWTOptionsToken);
builder.Services.AddDbContext<IStorageContext, ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
builder.Services.AddScoped<IStorage, DbStorage>();
builder.Services.AddSingleton<IRoomObserversProvider, RoomObserversProvider>();
builder.Services.AddScoped<IRoomsThreadsController, RoomsThreadsController>();
builder.Services.AddScoped<IRoomMusicPlayerController, RoomMusicPlayerController>();
builder.Services.AddScoped<IRoomMusicPlayerMusicProvider, RoomMusicPlayerMusicProvider>();

WebApplication app = builder.Build();

app.UseCors(builder => builder
    .WithOrigins("http://192.168.1.104:44430", "http://192.168.1.104:5092")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
);
app.UseMiddleware<JwtHeaderMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    //Secure = CookieSecurePolicy.Always
});
app.MapHub<RoomHub>("/hubs/room");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapFallbackToFile("index.html");
app.Run();
