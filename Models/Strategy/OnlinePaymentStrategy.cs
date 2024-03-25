using System;
using doan.Models.VnPay;
using Microsoft.AspNetCore.Http;

namespace doan.Models.Strategy
{
    public class OnlinePaymentStrategy : IPaymentStrategy
    {
        public string VnPayUrl { get; set; }

        public object Execute(Dondathang order, HttpContext context)
        {
            DotNetEnv.Env.Load();
            DotNetEnv.Env.TraversePath().Load();
            //Get Config Info
            string vnp_Returnurl = DotNetEnv.Env.GetString("vnp_Returnurl"); //URL nhan ket qua tra ve 
            string vnp_Url = DotNetEnv.Env.GetString("vnp_Url"); //URL thanh toan cua VNPAY 
            string vnp_TmnCode = DotNetEnv.Env.GetString("vnp_TmnCode"); //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = DotNetEnv.Env.GetString("vnp_hashSecret"); //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.TongDonHang * 100).ToString()); // Nhân cho 100 để thêm 2 số 0 :) 
            vnpay.AddRequestData("vnp_BankCode", "VNPAY");
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don dat hang:" + order.MaDdh);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); // Mã Website (Terminal ID)

            //Add Params of 2.1.0 Version
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return paymentUrl;
        }
    }
}
