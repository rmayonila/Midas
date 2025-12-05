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
            // Note: If 'AddDebug' is still red, delete the line below.
            builder.Logging.AddDebug();
#endif

            // DATABASE CONNECTION
            string connectionString = @"Server=LAPTOP-9GJ16B8S\SQLEXPRESS;Database=ERP_MIDAS;Trusted_Connection=True;TrustServerCertificate=True;";

            // REGISTER DB
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // REGISTER SERVICES
            // IMPORTANT: If this line is still red, go to your Data folder and check 
            // if the file is named "InventoryService.cs" (Singular) or "InventoryServices.cs" (Plural).
            // Make them match here!
            builder.Services.AddScoped<InventoryService>();
            builder.Services.AddScoped<PurchaseService>();
            builder.Services.AddScoped<SalesService>();
            builder.Services.AddScoped<ExpenseService>();
            builder.Services.AddScoped<PayrollService>();

            var app = builder.Build();

            // CREATE DB AUTOMATICALLY
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    // This prevents the app from crashing on startup if DB fails
                    Console.WriteLine($"DB Error: {ex.Message}");
                }
            }

            return app;
        }
    }
}