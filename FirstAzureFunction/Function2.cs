using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FirstAzureFunction.DataTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class Function2
    {
        // Visit https://aka.ms/sqlbindingsoutput to learn how to use this output binding
        [FunctionName("SetDataToDB")]
         public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "addNewCustomer")] HttpRequest req,
            [Sql("[dbo].[TestTable]", ConnectionStringSetting = "sqldb_connection")] IAsyncCollector<TestTable> testTableItems,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Output Binding function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TestTable testTableItem = JsonConvert.DeserializeObject<TestTable>(requestBody);

            await testTableItems.AddAsync(testTableItem);
            await testTableItems.FlushAsync();
            List<TestTable> resultList = new List<TestTable> { testTableItem };

            return new OkObjectResult(resultList);
        }
    }
}
