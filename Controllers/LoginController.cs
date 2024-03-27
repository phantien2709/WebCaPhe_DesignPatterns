using AspNetCoreHero.ToastNotification.Abstractions;
using doan.Controllers.AdapterAuthenticator;
using doan.Controllers.Proxy;
using doan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doan.Controllers
{
    public class LoginController:Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly StoreContext _context;
        private readonly IAuthenticationAdapter _authenticationAdapter;
        private readonly SecureProxy _secureProxy;

        public INotyfService _notyfyService { get; set; }
        public LoginController(ILogger<LoginController> logger, StoreContext context, INotyfService notifyService, IAuthenticationAdapter authenticationAdapter, SecureProxy secureProxy)
        {
            _logger = logger;
            _context = context;
            _notyfyService = notifyService;
            _authenticationAdapter = authenticationAdapter;
            _secureProxy = secureProxy;
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            var makh = HttpContext.Session.GetString("KhachHang");
            if (makh != null)
            {
                _notyfyService.Warning("Bạn đã đăng nhập. Hãy đăng xuất trước khi thực hiện nếu muốn.");
                return Redirect("/Home/Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            var makh = HttpContext.Session.GetString("KhachHang");
            if (makh != null)
            {
                _notyfyService.Warning("Bạn đã đăng nhập. Hãy đăng xuất trước khi thực hiện nếu muốn.");
                return Redirect("/Home/Index");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Dangnhap(string sdt, string pass)
        {
            var makh = HttpContext.Session.GetString("KhachHang");
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doan.Models.StoreContext)) as StoreContext;
            string hashedPassword = _secureProxy.HashPassword(pass);
            
            Taikhoan tk = _authenticationAdapter.Authenticate(sdt, pass);
            //Taikhoan tk1 = context.GetTaikhoan(sdt, pass);
            if (tk.MaTk != 0)
            {
                //Roles role = context.GetRoles(tk.RoleId);
                if (tk.RoleId.Equals(1))

                {
                    Khachhang kh = context.GetKhachHang(tk.SoDienThoai);
                    HttpContext.Session.SetString("KhachHang", kh.MaKh.ToString());
                    HttpContext.Session.SetString("TaiKhoan", tk.MaTk.ToString());
                    _notyfyService.Success("Đăng nhập thành công.");
                    try
                    {
                        _secureProxy.Authenticate(sdt, pass);
                    }
                    catch (Exception e)
                    {
                        pass = hashedPassword;
                        _secureProxy.Authenticate(sdt, pass);
                    }
                    return Redirect("/Home/Index");
                }
                else
                {
                    _logger.LogWarning("Login into Admin {0}", sdt);
                    _notyfyService.Success("Đăng nhập Admin thành công.");
                    // Sửa lại còn không là dính lỗi bảo mật 

                    return Redirect("/Admin");
                }
                
            }
            else
            {
                _logger.LogWarning("Failed login attempt for user with phone number: {0} : {1}", sdt,pass);
                _notyfyService.Error("Đăng nhập không thành công. Kiểm tra thông tin đăng nhập.");
                
                return RedirectToAction(nameof(SignIn));
            }            
        }
        [HttpPost]
        public IActionResult Dangky(string tenKH, string sdt, DateTime ngay, string gt, string diachi, string pass, string confirmpass)
        {
            if ((tenKH.Length > 50 || diachi.Length > 50) || (sdt.Length > 10 || pass.Length > 20))
            {
                _notyfyService.Error("Một số lỗi đã xảy ra. Vui lòng đăng ký lại.");
                return Redirect("/Login/SignUp");
            }
            if (pass.Equals(confirmpass))
            {
                try
                {
                    // Hash the provided password before storing it in the database
                    string hashedPassword = _secureProxy.HashPassword(pass);

                    StoreContext context = HttpContext.RequestServices.GetService(typeof(doan.Models.StoreContext)) as StoreContext;
                    Khachhang kh = context.GetKhachHang(sdt);
                    if (kh.MaKh != 0)
                    {
                        _notyfyService.Error("Số điện thoại đã đăng ký. Vui lòng dùng số khác.");
                        return Redirect("/Login/SignUp");
                    }
                    else
                    {
                        int check = context.insert_KhachHang(tenKH, sdt, ngay, gt, diachi);
                        if (check != 0)
                        {
                            int tmp = context.insert_TaiKhoan(sdt, hashedPassword);
                            if (tmp != 0)
                            {
                                Khachhang k = context.GetKhachHang(sdt);
                                HttpContext.Session.SetString("TaiKhoan", tmp.ToString());
                                HttpContext.Session.SetString("KhachHang", k.MaKh.ToString());
                                _notyfyService.Success("Đăng ký thành công.");
                                return Redirect("/Home/Index");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing registration for user with phone number: {0}", sdt);
                    _notyfyService.Error("Đã xảy ra lỗi không mong muốn. Vui lòng thử lại sau.");
                    return Redirect("/Login/SignUp");
                }
            }
            else
            {
                _notyfyService.Error("Xác nhận lại mật khẩu sai.");
                return Redirect("/Login/SignUp");
            }
            return Redirect("/Login/SignUp");
        }
        public IActionResult Signout()
        {
            var makh = HttpContext.Session.GetString("KhachHang");
            if (makh == null)
            {
                _notyfyService.Warning("Bạn chưa đăng nhập.");
                return Redirect("/Home/Index");
            }
            HttpContext.Session.Remove("KhachHang");
            _notyfyService.Success("Đăng xuất thành công.");
            return Redirect("/Home/Index");
        }
    }
}
