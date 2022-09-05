using e_website.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e_website.Models
{
    public class CartModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        //public int Price { get; set; }
    }
}