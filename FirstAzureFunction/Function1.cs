using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Collections.Generic;
using FirstAzureFunction.DataTypes;
using System.Linq;

namespace FirstAzureFunction
{
    public static class Function1
    {
        [FunctionName("GetDataFromDB")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Sql("SELECT * FROM [dbo].[test_table] where Code=@code",
            CommandType = System.Data.CommandType.Text,
            Parameters ="@code={Query.code}",
            ConnectionStringSetting = "sqldb_connection")] IEnumerable<Test_Table> result, 
            
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string code = req.Query["code"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            code = code ?? data?.code;

            string responseMessage = string.IsNullOrEmpty(code)
                ? "This HTTP triggered function executed and updated successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {result.FirstOrDefault().Username}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
