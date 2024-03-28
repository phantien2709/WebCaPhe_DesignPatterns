using global::doan.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Collections.Generic;
using doan.Controllers;
using doan.Models;
using System.Security.AccessControl;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace doan.Controllers.Decorator
{
    // Decorator class for bundled products
    public class BundleSanpham : IBundleSanpham
    {
        private readonly GioHangController _cartController;
        private readonly StoreContext _context;
        INotyfService _notifyService;
        // List of products in the bundle
        public List<Sanpham> Products { get; set; }
        public BundleSanpham() { }

        public BundleSanpham(GioHangController cartController, StoreContext context,INotyfService notyfService)
        {
            _cartController = cartController;
            Products = new List<Sanpham>();
            _context = context;
            _notifyService = notyfService;
        }

        // Method to add bundled products to the cart
        public void AddBundle(int productAmount)
        {
            // Call the Add_cart method of the GioHangController
            try
            {
                for (int i = 1; i <= productAmount; i++)
                {
                    _cartController.Add_cart(Products[i].MaSp);
                }
            }catch
            {
                _notifyService.Error("Không thể thêm sản phẩm vào giỏ hàng");
            }
            
        }
    }

}

