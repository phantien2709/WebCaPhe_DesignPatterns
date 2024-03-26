using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace doan.Models.Strategy
{
    public class NormalPaymentStrategy : IPaymentStrategy
    {
        public object Execute(Dondathang order, HttpContext context)
        {
            // Các giao tác của hoá đơn sẽ được
            // Controller xử lý do nó chỉ có lưu đơn hàng thành công         
            throw new System.NotImplementedException();
        }
    }
}
