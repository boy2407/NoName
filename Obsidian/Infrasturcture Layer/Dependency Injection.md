- **Vị trí**: Lưu tại file `DependencyInjection.cs` trên root của dự án `NoName.Infrastructure`.
- **Vai trò**: Cấu hình Inversion of Control (IoC). Đóng gói và đăng ký vòng đời Scoped/Transient/Singleton cho toàn bộ Infrastructure Layer lên WebAPI (Blazor/Backend).
- **Quy tắc bắt buộc**: Bất cứ file `.cs` nào với chức năng xử lý Logic, API, Database mới được tạo trong `/Persistence`, `/Services`, `/AIPlugins` đều **BẮT BUỘC** phải được thêm vào hàm cài đặt `IServiceCollection` trong class này.
### Mẫu DI
```csharp
public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
{
    // Cấu hình Entity Framework
    services.AddDbContext<NoNameDbContext>(...);
    
    // Đăng ký Unit of Work
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    
    // Đăng ký Repositories (Auto mapping bằng Reflection hoặc add thủ công)
    services.AddScoped<IOrderRepository, OrderRepository>();
    
    // Cấu hình AI & Redis
    ...
    return services;
}
```