using Microsoft.AspNetCore.Http;

namespace doan.Models.Strategy
{
    public interface IPaymentStrategy
    {
        object Execute(int amount, string ipv4);
    }
}
