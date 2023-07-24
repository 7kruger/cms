using System.Globalization;
using CourseWork;
using CourseWork.DAL;
using CourseWork.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = new List<CultureInfo>()
	{
		new CultureInfo("ru"),
		new CultureInfo("en")
	};

	options.DefaultRequestCulture = new RequestCulture("ru");
	options.SupportedCultures = supportedCultures;
	options.SupportedUICultures = supportedCultures;
});

var connection = builder.Configuration.GetConnectionString("LocalDB");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connection));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
		options.UseInMemoryDatabase(databaseName: "UnitTestsDatabase"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = new PathString("/Account/Login");
		options.AccessDeniedPath = new PathString("/Account/Login");
	});

builder.Services.AddMvc().AddViewLocalization();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.InitializeRepositories();
builder.Services.InitializeServices();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseRequestLocalization();

app.UseAuthentication();
app.UseMiddleware<CheckUserStatusMiddleware>();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");
	endpoints.MapRazorPages();
	endpoints.MapFallbackToPage("/index");
});

app.Run();