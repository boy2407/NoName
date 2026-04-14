- **Vị trí**: Lưu tại thư mục `/Persistence`.
- **Vai trò**: Chứa các class **Implementation** (thực thi) của `Repository Pattern` từ các Interfaces (ví dụ: `IOrderRepository`, `ITransactionRepository`) đã khai báo tại `[[Domain Layer]]` hoặc `[[Application Layer]]`.

### Quy tắc triển khai
1. **Kiểm tra Interface**: Phải check và implement đúng contract từ Application hoặc Domain. Không tự tiện viết method public thừa.
2. **C# 12 Primary Constructors**: Bắt buộc khởi tạo Controller/DI thông qua Primary Constructors thay vì viết khối constructor truyền thống.
   ```csharp
   public class OrderRepository(NoNameDbContext context) : IOrderRepository
   {
       public async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default)
       {
           return await context.Orders.FirstOrDefaultAsync(x => x.Id == id, ct);
       }
   }
   ```
3. **Tracking Data**: Tuyệt đối không gọi `.SaveChanges()` bên trong Repository. Toàn bộ transaction commit phải được thực hiện tại tầng Application thông qua `[[Unit of Work]]`.