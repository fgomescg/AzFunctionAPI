using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace FGomes.Function
{
    public static class ProductUpdate
    {
        [FunctionName("ProductUpdate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "product/{id:int}")] HttpRequest req,
            int id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var product = Db.GetProductById(id);
            
            if(product == null) {
                return new BadRequestObjectResult("Product not found!");     
            }

            if(name == null) {
                return new BadRequestObjectResult("Missing name in posted Body");
            }
            
            Db.UpdateProduct(id, name);

            return (ActionResult) new NoContentResult();
        }
    }
}