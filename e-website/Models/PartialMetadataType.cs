using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using e_website.Models;
using e_website.Context;

namespace e_website.Models


{
    [MetadataType(typeof(UserMasterData))]
    public partial class User
    {
        
    }

    [MetadataType(typeof(UserMasterData))]
    public partial class ProductMasterData
    {
        [NotMapped]
        public System.Web.HttpPostedFileBase ImageUpLoad { get; set; }
    }
}