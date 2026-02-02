using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using VetClinicCRM.Core.Interfaces;
using VetClinicCRM.Data.Data;
using VetClinicCRM.Data.Interfaces;
using VetClinicCRM.Data.Repositories;

namespace VetClinicCRM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IConfiguration Configuration { get; private set; }
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Построение конфигурации
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            // Настройка сервисов
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            // Запуск главного окна
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Регистрация DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Регистрация Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Регистрация репозиториев (если нужен прямой доступ)
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            services.AddScoped<IVaccinationRepository, VaccinationRepository>();
            services.AddScoped<IVaccineReminderRepository, VaccineReminderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Регистрация главного окна
            services.AddSingleton<MainWindow>();

            // Здесь позже зарегистрируем сервисы бизнес-логики
        }
    }

}
