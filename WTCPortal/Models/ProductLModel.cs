using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WTCPortal.Models
{
    public class ProductLModel
    {
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }

        public IEnumerable<Product> ProductList { get; set; }
        public Product Product { get; set; }
        public IEnumerable<Person> PersonList { get; set; }
        public ProductPhoto productPhoto { get; set; }
    }
}