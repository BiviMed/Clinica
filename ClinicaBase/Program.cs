using ClinicaBase.Data;
using ClinicaBase.Services.ServicioHash;
using ClinicaBase.Services.ServicioPacientes;
using ClinicaBase.Services.ServicioUsuarios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddHttpContextAccessor(); //PENDIENTE


builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add(new AuthorizeFilter(politicaUsuariosAutenticados));
});


builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IServicioUsuarios, ServicioUsuarios>();
builder.Services.AddScoped<IServicioHash, ServicioHash256>();
builder.Services.AddScoped<IServicioPaciente, ServicioPaciente>();


builder.Services.AddDbContext<ClinicaBase1Context>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});


builder.Services.AddAuthentication(options => {
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.Redirect("/Home/Index");
        return Task.CompletedTask; //con esto ya no me redirecciona a la pagia que viene por defecto
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Inicio}/{id?}");

app.Run();
