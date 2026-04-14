# BackendApi (`NoName.BackendApi`)

- **Vị trí**: `NoName.BackendApi` thuộc `[[Presentation Layer]]`.
- **Vai trò**: Điểm truy cập (Entry Point) công khai của hệ sinh thái phần mềm, nơi nhận các HTTP Request (GET/POST/PUT/DELETE) từ `AdminApp` (Blazor Wasm) hoặc Client ngoài.
- **Quy tắc thiết kế**:
  - Không code Logic (Business Rules) tại `Controller`. 
  - Controllers có nhiệm vụ cực kỳ mỏng: Ánh xạ HTTP Request thành cấu trúc Command/Query tương ứng của `[[CQRS and MediatR]]`. Gửi lệnh qua `await _mediator.Send(...)` và nhận kết quả để trả về HTTP Status code (200, 400).
  - Tích hợp **[[Dependency Injection]]** vào `Program.cs` (`AddApplication()`, `AddInfrastructure()`).
  - Kích hoạt cấu hình Swagger/OpenAPI.