To initiate the app you should proceed with the following steps:
Add a local.settings.json file in project with the DB connection string with the following format 
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "sqldb_connection": "..."
  }
}
Add your public ip in Azure Database as a new firewall rule. Find sql server resource -> click Networking -> Tab Public access -> Firewall Rules
