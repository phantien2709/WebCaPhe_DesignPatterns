using doan.Models;
using System.Collections.Generic;

namespace doan.Controllers.Decorator
{
    public interface IBundleSanpham
    {
        // List of products in the bundle
        List<Sanpham> Products { get; set; }
        void AddBundle();
    }
}
