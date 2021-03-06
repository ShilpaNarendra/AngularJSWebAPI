﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APM.WebAPI.Models
{
    public class Product
    {
        public string Description { get; set; }
       // [Range(100,1000,ErrorMessage="Price should be in range of 100 to 1000")]
        public decimal Price { get; set; }
        public string ProductCode { get; set; }
        public int ProductId { get; set; }
        [Required(ErrorMessage="Product Name is required",AllowEmptyStrings=false)]
        [MinLength(5,ErrorMessage="Product Name min length is 5 characters")]
        [MaxLength(7,ErrorMessage="Product Name max length is 7 characters")]
        public string ProductName { get; set; }
        public DateTime ReleaseDate { get; set; }

   
    }
}