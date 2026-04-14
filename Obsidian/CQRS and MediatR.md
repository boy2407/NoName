# CQRS & MediatR Pattern

Hệ thống NoName áp dụng chặt chẽ pattern theo cấu trúc `CQRS` + Thư viện `MediatR` tại `[[Application Layer]]`.

## Cơ chế
Tách rời trách nhiệm và logic (Separation of Concerns):
- **Command**: Thực hiện hành động thay đổi trạng thái (Tạo, Cập nhật, Xóa). Trả về thay đổi hay `Id`/`Result`.
- **Query**: Thực hiện thao tác truy xuất dữ liệu (Đọc). Không bao giờ thay đổi trạng thái.

## Quy tắc thiết kế (`copilot-instructions`)
- **Naming Convention (Bắt buộc)**:
    - `...Command` hoặc `...Query` (Yêu cầu gửi đi).
    - `...CommandHandler` hoặc `...QueryHandler` (Xử lý thực tiễn).
- **Validation (Kiểm tra)**: Mỗi Command phải có một class tương ứng triển khai `AbstractValidator<T>` (sử dụng thư viện `FluentValidation`). Validator sẽ được MediatR Pipeline chặn và thực hiện trước khi vào Handler.
- **Tối ưu Hóa (Read-Write Separation)**:
    - **Commands**: Thông qua `[[Repository Pattern]]` và EF Core Tracking để chỉnh sửa thực thể. Quản lý chung một `[[Unit of Work]]`.
    - **Queries**: Có thể chỉ dùng `.AsNoTracking()` trong Entity Framework hoặc truy vấn Dapper trực tiếp tuỳ theo yêu cầu hiệu năng để vượt qua tầng Entity cồng kềnh. KHÔNG gọi `SaveChanges` trong Query.

## Mã Code Mẫu (CQRS Pipeline)
```csharp
// 1. Định nghĩa Command
public record CreatePaymentLogCommand(Guid TransactionId, decimal Amount) : IRequest<Result<Guid>>;

// 2. Định nghĩa Validator
public class CreatePaymentLogCommandValidator : AbstractValidator<CreatePaymentLogCommand>
{
    public CreatePaymentLogCommandValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Số tiền phải lớn hơn 0");
    }
}

// 3. Xử lý Handler
public class CreatePaymentLogCommandHandler(
    ITransactionRepository transactionRepo, 
    IUnitOfWork unitOfWork) : IRequestHandler<CreatePaymentLogCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreatePaymentLogCommand request, CancellationToken ct)
    {
        // ... Logic xử lý
        // Có thể bắn Exception hoặc trả Result.Failure nếu TransactionId lỗi.
    }
}
```