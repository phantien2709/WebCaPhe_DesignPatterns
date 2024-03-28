using AspNetCore;
using doan.Controllers.Decorator;
using doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace doan.Controllers
{
    public class SanphamController : Controller
    {
        private readonly FivemenCoffeeContext _context;
        private readonly CacheDecorator _cacheDecorator;
        public SanphamController(FivemenCoffeeContext context)
        {
            _context = context;
            _cacheDecorator = new CacheDecorator();
            
        }
        public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 21;

                var models = _cacheDecorator.Execute(() =>
                {
                    var isProduct = _context.Sanpham.AsNoTracking().OrderBy(x => x.MaSp);
                    return new PagedList<Sanpham>(isProduct, pageNumber, pageSize);
                }, "Index");

                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("/{id}/{tendanhmuc}.html")]
        public IActionResult List(int id, int page = 1)
        {
            try
            {
                var pageSize = 9;
                var category = _context.Danhmucsp.Find(id);

                var models = _cacheDecorator.Execute(() =>
                {
                    var isProduct = _context.Sanpham.AsNoTracking()
                        .Where(x => x.MaDanhMuc == id)
                        .OrderBy(x => x.MaSp);
                    return new PagedList<Sanpham>(isProduct, page, pageSize);
                }, "List", id, page);

                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = category;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("/{id}.html")]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _cacheDecorator.Execute(() =>
                {
                    return _context.Sanpham.Include(x => x.MaDanhMucNavigation).FirstOrDefault(x => x.MaSp == id);
                }, "Details", id);

                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                var lsProduct = _cacheDecorator.Execute(() =>
                {
                    return _context.Sanpham.AsNoTracking()
                        .Where(x => x.MaDanhMuc == product.MaDanhMuc && x.MaSp != id)
                        .OrderBy(x => x.MaSp)
                        .Take(4)
                        .ToList();
                }, "RelatedProducts", id);

                ViewBag.Sanpham = lsProduct;
                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
