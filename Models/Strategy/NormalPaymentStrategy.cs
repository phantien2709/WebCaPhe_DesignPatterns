using Microsoft.AspNetCore.Http;

namespace doan.Models.Strategy
{
    public class NormalPaymentStrategy : IPaymentStrategy
    {
        public object Execute(int amount, string ipv4)
        {
            // Các giao tác của hoá đơn sẽ được
            // Controller xử lý do nó chỉ có lưu đơn hàng thành công         
            throw new System.NotImplementedException();
        }
    }
}
