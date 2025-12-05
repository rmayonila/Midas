using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MDAS.Data;

namespace MDAS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            // REMOVED THE AddDebug LINE CAUSING ERRORS
#endif

            // 1. DATABASE CONNECTION
            string connectionString = @"Server=LAPTOP-9GJ16B8S\SQLEXPRESS;Database=ERP_MIDAS;Trusted_Connection=True;TrustServerCertificate=True;";

            // 2. REGISTER DB CONTEXT
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 3. REGISTER SERVICES
            builder.Services.AddScoped<InventoryServices>();
            builder.Services.AddScoped<PurchaseService>();

            var app = builder.Build();

            // 4. ENSURE DATABASE IS CREATED
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database Error: {ex.Message}");
                }
            }

            return app;
        }
    }
}