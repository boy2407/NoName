using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Services
{
    public interface  IPaymentService
    {
        string ProviderName { get; }
        Task<string> CreatePaymentAsync(Order order);
        Task<bool> ValidateCallback(IDictionary<string, string> queryParams);
    }
}
