# Shared Layer - Models & DTOs

- **Vị trí**: `NoName.Shared`.
- **Vai trò**: Cầu nối duy nhất giữa `[[Application Layer]]` (và BackendApi) và `[[Blazor Wasm UI]]` (Frontend). Chuyên chở `DTO`, `ApiRoutes` và Constants dùng chung.
- **Quy tắc thiết kế**:
  - Tuyệt đối không chứa logic xử lý (Business Rules) hay Mapping DB. 
  - File tạo ở đây là thuần dữ liệu POCOs.
  - Ví dụ: Các tham số như chuỗi `/api/v1/orders/` nên đưa vào biến `ApiRoutes.Orders.GetAll` thuộc file tĩnh ở thư mục này.
  - Ngăn ngừa tình trạng Frontend và Backend chênh lệch tham số Payload (Request Model).