# Quản lý Services Ngoại vi

- **Vị trí**: `/Services` thuộc `[[Infrastructure Layer]]`.
- **Vai trò**: Triển khai các Abstract Services (IPaymentService, IEmailService, IStorageService) của tầng Application (`[[Application Layer]]`).
- **Quy tắc thiết kế (`copilot-instructions`)**:
  - Tích hợp các SDK thứ 3 (MoMo, Stripe, SendGrid, Amazon S3, Redis RedisRedlock) ở đây.
  - Phải tách biệt trách nhiệm: Code không làm chức năng logic dữ liệu của Domain (VD: Không kiểm duyệt đơn thanh toán có hợp lệ hay không ở MomoService, điều đó thuộc về PaymentHandler).
  - Tích hợp `ICacheService` (cho `[[Concurrency Management]]`). Không để logic Cache rò rỉ ra API Controller hay CQRS Handlers.
  - Bắt buộc phải thêm Scoped/Singleton tại `[[Dependency Injection]]`.