using global::doan.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Collections.Generic;
using doan.Controllers;
using doan.Models;
using System.Security.AccessControl;

namespace doan.Controllers.Decorator
{
    // Decorator class for bundled products
    public class BundleSanpham : IBundleSanpham
    {
        private readonly GioHangController _cartController;
        private readonly StoreContext _context;
        // List of products in the bundle
        public List<Sanpham> Products { get; set; }
        public BundleSanpham() { }

        public BundleSanpham(GioHangController cartController, StoreContext context)
        {
            _cartController = cartController;
            Products = new List<Sanpham>();
            _context = context;
        }

        // Method to add bundled products to the cart
        public void AddBundle()
        {
           
            // Call the Add_cart method of the GioHangController
            for (int i = 1; i <= Products.Count; i++)
            {
                _cartController.Add_cart(Products[i].MaSp);
            }
            
        }
    }

}

