using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FGomes.Function
{
    public static class ProductDelete
    {
        [FunctionName("ProductDelete")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "product/{id:int}")] HttpRequest req,
            int id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var product = Db.GetProductById(id);

            if(product == null) {
                return new BadRequestObjectResult("Product not found!");     
            }

            Db.DeleteProduct(product);           

            return (ActionResult) new NoContentResult();
        }
    }
}