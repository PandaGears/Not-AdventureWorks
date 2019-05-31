using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WTCPortal.Controllers;
using WTCPortal.Repository;
using WTCPortal.Models;

namespace WTCPortal.ViewModel
{
    public class ProductViewModel
    {
        ProductController _controller = new ProductController();
        ProductLModel _viewModel = new ProductLModel();

        public ProductViewModel()
        {

        }

        public ProductViewModel(ProductController controller, ProductLModel viewModel)
        {

        }

        public ProductLModel Build()
        {
            _viewModel.PageTitle = "Employee List";
            _viewModel.MetaDescription = "This is the Employee List Page";

            var repo = new ProductRepo("AdventureWorks");

            _viewModel.ProductList = repo.GetAll();
            return _viewModel;
        }

          public ProductLModel DBuild(int ProductID)
           {
               var repo = new ProductRepo("AdventureWorks");
               _viewModel.Product = repo.GetById(ProductID);
               return _viewModel;
           }

    }
}