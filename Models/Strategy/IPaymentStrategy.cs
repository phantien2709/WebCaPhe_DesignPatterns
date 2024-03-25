using Microsoft.AspNetCore.Http;

namespace doan.Models.Strategy
{
    public interface IPaymentStrategy
    {
        object Execute(Dondathang order, HttpContext context);
    }
}
