using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doan.Models
{
    public class ProductPrototype
    {
        private static ProductPrototype instance;
        private readonly FivemenCoffeeContext database = new FivemenCoffeeContext();
        private List<Sanpham> products;

        private ProductPrototype()
        {
            products = database.Sanpham.ToList();
        }

        public static ProductPrototype GetInstance()
        {
            if (instance == null)
            {
                instance = new ProductPrototype();
            }
            return instance;
        }

        public List<Sanpham> Products
        {
            get { return products; }
        }
        public void AddProduct(Sanpham product)
        {
            // Thực hiện sao chép (clone) đối tượng sản phẩm
            var clonedProduct = CloneProduct(product);

            // Thêm sản phẩm vào danh sách và cơ sở dữ liệu
            products.Add(clonedProduct);
            database.Sanpham.Add(clonedProduct);
            database.SaveChanges();
        }

        private Sanpham CloneProduct(Sanpham originalProduct)
        {
            // Tạo một bản sao mới của sản phẩm
            var clonedProduct = new Sanpham
            {
                MaSp = originalProduct.MaSp,
                TenSp = originalProduct.TenSp,
                GiaTien = originalProduct.GiaTien,
                SoLuong = originalProduct.SoLuong,
                MaDanhMuc = originalProduct.MaDanhMuc,
                MoTa = originalProduct.MoTa,
                MaNcc = originalProduct.MaNcc,
            };
            return clonedProduct;
        }
    }
}