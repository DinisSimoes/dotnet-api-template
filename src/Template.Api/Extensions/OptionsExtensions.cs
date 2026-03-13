namespace Template.Api.Extensions
{
    public static class OptionsExtensions
    {
        public static IServiceCollection ConfigureOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // exemplo
            // services.Configure<MyOptions>(configuration.GetSection("MyOptions"));

            return services;
        }
    }
}
