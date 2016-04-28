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
using System.Web.Http.Description;
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
    // [EnableQuery(AllowedQueryOptions =
    //AllowedQueryOptions.OrderBy | AllowedQueryOptions.Filter)]
    //   public IQueryable<Product> Get()
    //   {
    //       var productRepo = new ProductRepository();
    //       return productRepo.Retrieve().AsQueryable();
    //   }

       [EnableQuery(AllowedQueryOptions =
        AllowedQueryOptions.OrderBy | AllowedQueryOptions.Filter)]
       [ResponseType(typeof(Product))]
       public IHttpActionResult Get()
       {
           try
           {
               var productRepo = new ProductRepository();
               return Ok(productRepo.Retrieve().AsQueryable());
           }
           catch(Exception ex)
           {
               return InternalServerError(ex);
           }
       }
        //public IEnumerable<Product> Get(string search)
        //{
        //    var productRepo = new ProductRepository();
        //    var product = productRepo.Retrieve();
        //    return product.Where(p => p.ProductCode.Contains(search));
        //}
       [ResponseType(typeof(Product))]
       public IHttpActionResult Get(string search)
       {
           try
           { 
           var productRepo = new ProductRepository();
           var product = productRepo.Retrieve();
           return Ok(product.Where(p => p.ProductCode.Contains(search)));
           }
           catch (Exception ex)
           {
               return InternalServerError(ex);
           }
       }

        // GET api/products/5
       [ResponseType(typeof(Product))]
       public IHttpActionResult Get(int id)
       {
           try
           {
               //throw new ArgumentNullException("this is a test message");
               Product product;
               var productRepo = new ProductRepository();
               var products = productRepo.Retrieve();
               if (id > 0)
               {
                   product = products.FirstOrDefault(p => p.ProductId == id);
                   if (product == null)
                       return NotFound();
                   else
                       return Ok(product);
               }
               else
               {
                   product = productRepo.Create();
                   return Ok(product);
               }
           }
           catch (Exception ex)
           {
               return InternalServerError(ex);
           }
       }   

        // POST api/products
        public IHttpActionResult Post([FromBody]Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Product cannot be null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var productRepo = new ProductRepository();
                var newProduct = productRepo.Save(product);
                if (newProduct == null)
                {
                    return Conflict();
                }
                return Created<Product>(Request.RequestUri + newProduct.ProductId.ToString(), newProduct);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT api/products/5
        public IHttpActionResult Put(int id, [FromBody]Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Product cannot be null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var productRepo = new ProductRepository();
                var updatedProduct = productRepo.Save(id, product);
                if (updatedProduct == null)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE api/products/5
        public void Delete(int id)
        {
        }
    }
}
