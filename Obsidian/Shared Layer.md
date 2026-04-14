# Shared Layer (`NoName.Shared`)

Đặc sản của mô hình **Blazor WebAssembly + .NET WebAPI** là có thể chia sẻ trực tiếp code C# giữa Frontend và Backend. `NoName.Shared` ra đời cho mục đích này.

## Quy tắc thiết kế
- **Không chứa logic nghiệp vụ (Business Logic)**: Chỉ chứa dữ liệu thuần.
- **Không phụ thuộc vào tầng khác**: Không reference đến `Domain`, `Application`, hay `Infrastructure` để tránh rò rỉ (leak) logic ra bên ngoài Frontend.

## Các thành phần chính:
1. **Data Transfer Objects (DTOs) & ViewModels**:
   - Chứa các class dữ liệu để Payload/Return qua API. (Ví dụ: `ProductDto`, `OrderResponse`, `CreateOrderRequest`).
2. **Constants & Route Templates**:
   - Lưu trữ các hằng số dùng chung, các chuỗi định tuyến API (API Endpoints) để Frontend gọi API không bị sai chính tả. (Ví dụ: `ApiRoutes.Orders.Create`).
3. **Shared Enums**:
   - Có thể chứa các Enum chung bắt buộc hiển thị trên UI (Tuy nhiên một số dự án để Enum ở Domain, cần đối chiếu cấu trúc thực tế).
4. **Wrapper / Result Pattern**:
   - Chứa các class bọc kết quả trả về như `ApiResult<T>`, `PagedResult<T>` để chuẩn hoá format Error/Success cho tất cả các API.