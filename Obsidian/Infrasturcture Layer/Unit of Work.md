- **Purpose:** Đảm bảo tất cả các thay đổi trong một Business Transaction được lưu vào DB cùng lúc thông qua `[[EF Core]]`.
### Implement Unit of Work với EF Core
```csharp
public class Unit of Work : IUnitOfWork {
    private readonly MyDbContext _context;
    public UnitOfWork(MyDbContext context) => _context = context;
    
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
```