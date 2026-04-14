# Blazor WebAssembly (`NoName.AdminApp`)

- **Vị trí**: `NoName.AdminApp` thuộc `[[Presentation Layer]]`.
- **Vai trò**: Cung cấp giao diện quản trị WebAssembly cho Admin, chạy toàn bộ trên trình duyệt Client-side.
- **Quy tắc thiết kế**:
  - Không bao giờ để lộ các giá trị nhạy cảm (Secret Keys / ConnectionStrings / Admin API keys) tại `appsettings.json` của Blazor Wasm vì Client có thể đọc tất cả mã nguồn.
  - Tất cả Request gọi đến `BackendApi` phải đi qua một Service (`IProductApiClient`, `IOrderApiClient`) đóng gói thư viện `HttpClient` và nối Token (JWT Bearer Token).
  - Tái sử dụng `Components` để tổ chức HTML một cách nhẹ nhất, cấu trúc theo thư mục `Pages`/`Components`.
  - Tuân thủ quy tắc DTO từ `[[Shared Layer]]`. Dữ liệu trả về luôn phải bọc trong mô hình Result để kiểm tra tình trạng lỗi tập trung (Error Boundaries / Interceptors).