# Domain Layer

Tầng cốt lõi của **Clean Architecture**, nơi không có bất cứ phụ thuộc nào vào các công nghệ cụ thể (No Entity Framework, no MediatR, no ASP.NET Core). Nó chứa toàn bộ Trạng thái (State) và Quy tắc Tên miền (Domain Rules) của NoName Product.

## Các thành phần chính và thư mục tương ứng:

1. **Thực thể dữ liệu (`/Entities`)**
   - Chứa các POCOs class ánh xạ thành các bảng trong CSDL (Ví dụ: `Order`, `Transaction`, `User`).
   - Mọi khóa chính, quan hệ navigation đều được thiết lập tại đây, tuy nhiên để *Cấu hình DB* thì phải làm qua `[[EF Core]]` bên tầng Infrastructure.
   - Các Entity luôn ưu tiên trạng thái encapsulation (ví dụ đóng private set properties nếu có logic thay đổi phức tạp).

2. **Dữ liệu cố định (Enumerations) (`/Enums`)**
   - Lưu trữ các Enum để biểu đạt trạng thái hay loại dữ liệu rõ ràng thay vì Hardcode String. (Ví dụ: `TransactionStatus`, `OrderStatus`).
   - Nên chuyển Enum thành chuỗi khi lưu trên DB thay vì số (qua Fluent API struct) để dễ đọc log.

3. **Domain Events & Exceptions (tuỳ chọn)**
   - Nơi bắn Exception gắn liền với nghiệp vụ thuần khiết.

## Quy tắc thiết kế
- Tuyệt đối **KHÔNG DI** service infra vào domain. Trách nhiệm chỉ là mô phỏng hiện thực (Model the reality).
- Mọi Interface Repository sẽ **không nằm** ở đây mà thường nằm ở `[[Application Layer]]` phần `Abstractions`.