using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using GeliconProject.Middlewares;
using GeliconProject.Utils.JWTValidationParameters;
using GeliconProject.Utils.ApplicationContexts;
using GeliconProject.Hubs;

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
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
