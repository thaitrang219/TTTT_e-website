using e_website.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e_website.Models
{
    public class HomeModel
    {
        public List<Category> ListCategory { get; set; }
        public List<Product> ListProduct { get; set; }
        
    }
}