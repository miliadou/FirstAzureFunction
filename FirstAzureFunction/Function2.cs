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
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "addNewUser")] HttpRequest req,
            [Sql("[dbo].[test_table]", ConnectionStringSetting = "sqldb_connection")] IAsyncCollector<Test_Table> testTableItems,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Output Binding function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Test_Table testTableItem = JsonConvert.DeserializeObject<Test_Table>(requestBody);

            await testTableItems.AddAsync(testTableItem);
            await testTableItems.FlushAsync();
            List<Test_Table> resultList = new List<Test_Table> { testTableItem };

            return new OkObjectResult(resultList);
        }
    }
}
