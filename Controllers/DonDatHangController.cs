using AspNetCoreHero.ToastNotification.Abstractions;
using doan.Models;
using doan.Models.Strategy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace doan.Controllers
{
    public class DonDatHangController : Controller
    {
        private readonly StoreContext _context;
        public INotyfService _notyfyService { get; set; }

        public DonDatHangController(StoreContext context, INotyfService notifyService)
        {
            _context = context;
            _notyfyService = notifyService;
        }

        public IActionResult Index_DDH()
        {
            var makh = HttpContext.Session.GetString("KhachHang");
            if (makh != null)
            {
                StoreContext context = HttpContext.RequestServices.GetService(typeof(StoreContext)) as StoreContext;
                if (Request.Cookies["GioHang"] != null)
                {
                    var cart = Request.Cookies["GioHang"];
                    List<GioHang> dataCart = JsonConvert.DeserializeObject<List<GioHang>>(cart);
                    if (dataCart.Count > 0)
                    {
                        ViewBag.carts = dataCart;
                    }
                }
                ViewBag.nhavanchuyen = context.getNVC();
                return View();
            }
            else
            {
                return Redirect("/Login/SignIn");
            }
                
        }
        [HttpPost]
        public IActionResult DatHang(string ten, string sdt, string diachi, int nvc, int paymentMode)
        {
            if ((ten.Length > 50 || diachi.Length > 50) || (sdt.Length > 10))
            {
                _notyfyService.Error("Một số lỗi đã xảy ra. Có vẻ bạn đã nhập sai thông tin giao hàng.");
                return Redirect("/Giohang/Index_GioHang");
            }
            StoreContext context = HttpContext.
                    RequestServices.GetService(typeof(StoreContext)) as StoreContext;
            // Chọn Strategy dựa trên lựa chọn của người dùng
            PaymentContext paymentContext = new PaymentContext(context);
            if (paymentMode != 0)
            {
                paymentContext.Strategy = new OnlinePaymentStrategy();
            }
            if (Request.Cookies["GioHang"] != null)
            {
                var cart = Request.Cookies["GioHang"];
                List<GioHang> dataCart = JsonConvert.DeserializeObject<List<GioHang>>(cart);
                if (dataCart.Count > 0)
                {
                    ViewBag.carts = dataCart;
                }
                string kh = HttpContext.Session.GetString("KhachHang");
                if (kh != null)
                {
                    int maKH = Convert.ToInt32(kh);
                    int maddh = paymentContext.SaveOrder(maKH, dataCart, nvc);
                    if (maddh != 0)
                    {
                        ViewData["MADDH"] = maddh;
                        Response.Cookies.Delete("GioHang");
                    }
                    else
                    {
                        _notyfyService.Error("Đã có lỗi xảy ra.");
                        return Redirect("/Error404/Page404");
                    }
                }
                return View();
            }
            else
            {
                _notyfyService.Error("Đã có lỗi xảy ra.");
                return Redirect("/Error404/Page404");
            }
        } 
    }
}
