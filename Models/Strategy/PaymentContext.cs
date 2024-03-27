using System;
using System.Collections.Generic;

namespace doan.Models.Strategy
{
    public class PaymentContext
    {
        public StoreContext Context { get; set; }
        public IPaymentStrategy Strategy { get; set; }

        public PaymentContext(StoreContext context)
        {
            this.Context = context;
            this.Strategy = new NormalPaymentStrategy();
        }

        public int GetTotalAmount(List<GioHang> dataCart)
        {
            int tongtien = 0;
            foreach (var item in dataCart)
            {
                tongtien += Convert.ToInt32(item.Soluong * item.sanpham.GiaTien);
            }

            return tongtien;
        }
        public (int, int) SaveOrder(int customerId, List<GioHang> dataCart, int idShipper)
        {
            try
            {
                int sump = 0;
                int maddh = this.Context.insert_DDH(customerId,
                this.GetTotalAmount(dataCart), DateTime.Now, idShipper);
                foreach (GioHang item in dataCart)
                {
                    sump += Convert.ToInt32(item.Soluong * item.sanpham.GiaTien);
                    int tmp = this.Context.insert_CTDH(
                        maddh, item.sanpham.MaSp, item.Soluong, sump
                    );
                    Sanpham dataSPnew = this.Context.Product_id(item.sanpham.MaSp);
                    this.Context.update_SanPham(item.sanpham.MaSp, dataSPnew.SoLuong - item.Soluong);
                }

                return (maddh, sump);
            }
            catch
            {
                return (0, 0);
            }
        }
    }
}
