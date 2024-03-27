using AspNetCoreHero.ToastNotification.Abstractions;
using BCrypt.Net;
using doan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doan.Controllers
{
    public class TaiKhoanKHController: Controller
    {
        private readonly StoreContext _context;
        public INotyfService _notyfyService { get; set; }
        public TaiKhoanKHController(StoreContext context, INotyfService notifyService)
        {
            _context = context;
            _notyfyService = notifyService;
        }
        [HttpGet]
        public IActionResult Index_TaiKhoan()
        {
            var makh = HttpContext.Session.GetString("KhachHang");
            if (makh == null)
            {
                _notyfyService.Warning("Bạn chưa đăng nhập.");
                return Redirect("/Home/Index");
            }
            else
            {
                StoreContext context = HttpContext.RequestServices.GetService(typeof(doan.Models.StoreContext)) as StoreContext;
                ViewBag.Khachhang = context.GetKhachHangbyid(Convert.ToInt32(makh));
                ViewBag.Dondathang = context.GetDonHangbyidKH(Convert.ToInt32(makh));
                return View();
            }            
        }
        [HttpPost]
        public IActionResult DoiPass(string pass, string passwordtk, string confirmpasswordtk)
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doan.Models.StoreContext)) as StoreContext;
            var tk = HttpContext.Session.GetString("TaiKhoan");
            int matk = Convert.ToInt32(tk);

            // Retrieve the current password from the database
            Taikhoan taikhoan = context.GetTaikhoanbyid(matk);
            string currentPassword = taikhoan.MatKhau;
            string inputcurrentPass = BCrypt.Net.BCrypt.HashPassword(pass);

            try
            {
                // Check if the current password is hashed (BCrypt)
                
                bool isCurrentPasswordHashed = BCrypt.Net.BCrypt.Verify(pass, currentPassword);
                // Hash the new password if the current password is hashed
               // string newPassword = isCurrentPasswordHashed ? BCrypt.Net.BCrypt.HashPassword(passwordtk) : passwordtk;
                    
                if (isCurrentPasswordHashed)
                {
                    if (passwordtk.Equals(confirmpasswordtk))
                    {
                        if (passwordtk.Length > 20)
                        {
                            _notyfyService.Error("Đã có lỗi xảy ra.");
                            return Redirect("/TaiKhoanKH/Index_TaiKhoan");
                        }
                        string newPassword = BCrypt.Net.BCrypt.HashPassword(passwordtk);
                        // Update the password in the database
                        int result = context.DoiPass(matk, newPassword);

                        if (result != 0)
                        {
                            _notyfyService.Success("Đổi mật khẩu thành công.");
                        }
                        else
                        {
                            _notyfyService.Error("Đổi mật khẩu thất bại.");
                        }
                    }
                    else
                    {
                        _notyfyService.Error("Xác nhận lại mật khẩu sai.");
                    }
                }
                else
                {
                    _notyfyService.Error("Mật khẩu hiện tại sai.");
                }
            }
            catch (System.FormatException)
            {
                // Handle the case where the current password is not hashed (plain text)
                // Implement appropriate error handling or logging
                _notyfyService.Error("Lỗi xảy ra khi xác định loại mật khẩu.");
            }

            return Redirect("/TaiKhoanKH/Index_TaiKhoan");
        }

    }
}
