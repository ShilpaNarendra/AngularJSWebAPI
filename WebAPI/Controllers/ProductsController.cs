using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APM.WebAPI.Models;
using System.Web.Http.Cors;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
//using System.Web.OData;
//using System.Web.Http.Cors;   


namespace APM.WebAPI.Controllers
{
   [EnableCorsAttribute("*", "*", "*")]
    public class ProductsController : ApiController
    {
        // GET api/products

       //public IEnumerable<Product> Get()
       //{
       //    var productRepo = new ProductRepository();
       //    return productRepo.Retrieve();
       //}
     [EnableQuery(AllowedQueryOptions =
    AllowedQueryOptions.OrderBy | AllowedQueryOptions.Filter)]
       public IQueryable<Product> Get()
       {
           var productRepo = new ProductRepository();
           return productRepo.Retrieve().AsQueryable();
       }

        public IEnumerable<Product> Get(string search)
        {
            var productRepo = new ProductRepository();
            var product = productRepo.Retrieve();
            return product.Where(p => p.ProductCode.Contains(search));
        }

        // GET api/products/5
        public Product Get(int id)
        {
            var productRepo = new ProductRepository();
            var products = productRepo.Retrieve();
            if (id > 0)
                return products.FirstOrDefault(p => p.ProductId == id);
            else
            {
                var product = productRepo.Create();
                return product;
            }
        }   

        // POST api/products
        public void Post([FromBody]Product product)
        {
            var productRepo = new ProductRepository();
            var newProduct= productRepo.Save(product);
        }

        // PUT api/products/5
        public void Put(int id, [FromBody]Product product)
        {
            var productRepo = new ProductRepository();
            var newProduct = productRepo.Save(id,product);
        }

        // DELETE api/products/5
        public void Delete(int id)
        {
        }
    }
}
