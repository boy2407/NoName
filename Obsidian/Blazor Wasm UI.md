# Blazor WebAssembly UI (`NoName.AdminApp`)

Giao diện hệ thống quản lý chính cho **NoName Product**, sử dụng nền tảng **.NET 8 Blazor WebAssembly**.

## Nguyên tắc tổ chức

Hiệu năng và trải nghiệm cho ứng dụng Client-side dựa trên các luồng sau:
1. **Chia nhỏ Thành Phần (Component-based):**
   - **`Pages/`** (Views): Chứa các component đại diện cho Router. Các page chỉ làm nhiệm vụ kết nối UI -> Service -> State.
   - **`Components/`**: Các thành phần tái sử dụng (Bảng, Modal, Form nhập liệu, Select list). Các component này chỉ nhận `[Parameter]` và bắn UI Events `[Parameter] EventCallback`.
   
2. **Quản lý Trạng Thái & Logic gọi API:**
   - **`Services/`**: Gọi HttpClients để giao tiếp với `NoName.BackendApi`. Thay vì viết `HttpClient.GetAsync` chằng chịt trong component, hãy trừu tượng hoá vào: `IProductApiClient`, `IOrderApiClient`.
   - **Gắn Token Bearer**: Mọi HttpClient phải thiết lập `HttpMessageHandler` tự động đính kèm Access Token lấy từ LocalStorage (hoặc AuthenticationStateProvider).
   - **Xử lý Loading / Lỗi**: Dùng Interceptor, Middleware chung hoặc Error Boundary ở layout cao nhất để bắt lỗi 401, 403, 500 thay vì xử lý riêng lẻ 100 lần ở 100 components.

3. **Routing & Authentication:**
   - Sử dụng `AuthorizeView`, `[Authorize(Roles="Admin")]` trong Pages để bảo mật truy cập.
   - Thao tác đăng nhập kết nối qua Custom `AuthenticationStateProvider`.

## Tránh các lỗi phổ biến (Anti-patterns):
- Liên kết (Reference) trực tiếp tới dự án `EFCore`, `Infrastructure` thay vì dùng dự án API.
- Để lộ (Leak) các Secret Keys / Mật khẩu Database trong `appsettings.json` của Blazor Wasm. **TẤT CẢ CONFIG Ở BLAZOR CÓ THỂ BỊ ĐỌC BỞI CLIENT**.

## Code Standards
- Dữ liệu binding (Models): Cấu trúc Request/Response và Form binding nên lấy từ `[NoName.Shared]([[Shared Layer]])`.