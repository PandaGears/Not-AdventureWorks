using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WTCPortal.Models;
using WTCPortal.ViewModel;

namespace WTCPortal.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(string searchString)
        {
            ProductLModel viewModel = new ProductLModel();
            ProductViewModel productViewer = new ProductViewModel(this, viewModel);

            viewModel = productViewer.Build();
            var ProductRes = viewModel.ProductList;
            if (!String.IsNullOrEmpty(searchString))
            {
                viewModel.ProductList = ProductRes.Where(str => str.Name5.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return View(viewModel);
        }

        public ActionResult Details(int Productid)
        {
            ProductLModel viewModel = new ProductLModel();

            ProductViewModel employeeViewModel = new ProductViewModel(this, viewModel);

            viewModel = employeeViewModel.DBuild(Productid);

            return View(viewModel);
        }

        public ActionResult ShowPic(int id)
        {
            ProductLModel viewModel = new ProductLModel();

            ProductViewModel employeeViewModel = new ProductViewModel(this, viewModel);

            viewModel = employeeViewModel.DBuild(id);
            byte[] imageData = viewModel.Product.LargePhoto;
            return File(imageData, "image/gif");
        }

    }
}