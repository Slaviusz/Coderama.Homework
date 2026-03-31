namespace Coderama.Homework.Mappers;

internal static class DependencyInjectionExtensions {
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        var mapperTypes = typeof(Program).Assembly.GetTypes()
            .Where(
                t => t is { Namespace: "Coderama.Homework.Mappers", IsInterface: false, IsAbstract: false }
                && t.Name.EndsWith("Mapper")
                && t.GetMethods().Any(m => m.Name.StartsWith("MapTo"))
            );

        foreach (var type in mapperTypes)
        {
            services.AddSingleton(type);
        }

        return services;
    }
}
