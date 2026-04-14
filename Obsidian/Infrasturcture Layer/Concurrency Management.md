- **Vị trí**: Quản lý tại `[[Infrastructure Layer]]`.
- **Vai trò**: Tối ưu tốc độ hệ thống bằng Redis hoặc xử lý xung đột trong mua bán (Ví dụ: 2 user mua cùng lúc 1 sản phẩm chỉ còn 1 tồn kho).
- **Quy tắc (Theo `copilot-instructions`)**:
  - Không nhét logic Caching vào Controller/Handler. Xử lý qua thư viện trung gian (như `ICacheService`).
  - **Xử trí Race Condition cho Inventory (Kho hàng)**: Sử dụng mô hình **Redis RedLock**.
  
### Flow Redis RedLock
1. Ai Request tới sớm nhất sẽ **đoạt được (Acquire) Lock**.
2. Request thứ 2 sẽ nhường quyền và được chuyển trạng thái sang **Chờ (Wait)**.
3. System tiến hành **Retry tự động** theo các đoạn thời gian ngắn (ví dụ mỗi **50ms**).
4. Nếu user thứ 1 thanh toán xong / huỷ giỏ -> giải phóng Lock -> user thứ 2 lấy được Lock. Hoặc hết thời hạn `Timeout` -> user thứ 2 bị đá văng (Báo lỗi đã hết hàng).