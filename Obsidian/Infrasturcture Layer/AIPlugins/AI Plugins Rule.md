# Quản lý AI Plugins / Agents

- **Vị trí**: `/AIPlugins` thuộc `[[Infrastructure Layer]]`.
- **Vai trò**: Cung cấp mã cho các AI Agent (Copilot) tích hợp sâu vào quy trình hệ thống backend NoName.
- **Quy tắc thiết kế (`copilot-instructions`)**:
  - Dùng `[[Semantic Kernel]]` hoặc Call API từ `Ollama`.
  - Plugins AI được đăng ký thông qua `IServiceCollection` tại `[[Dependency Injection]]`.
  - Mọi API key/Endpoint cấu hình tại `appsettings.json` phải có lớp Model Strongly-Typed map `(ví dụ: AIConfigModel)` và inject qua `IOptions`.
  - Hỗ trợ Agent tương tác nội bộ: Có thể gọi trực tiếp `IGenericRepository` vào Plugin (ví dụ `AdminPlugin`) hoặc Inject Command/Query nếu Agent cần trích xuất Dashboard.