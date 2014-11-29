#r "Microsoft.WindowsAzure.ServiceRuntime.dll"
#r @"..\packages\WindowsAzure.Storage.4.3.0\lib\net40\Microsoft.WindowsAzure.Storage.dll" 
 
open System
open System.IO
open System.Linq
open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Table

let accountName = ""
let accountKey = ""
let url =  ""

let credentials = new Auth.StorageCredentials(accountName, accountKey)
let storageAccount = new CloudStorageAccount(credentials, true)
storageAccount.TableEndpoint = Uri(url)
let tableClient = storageAccount.CreateCloudTableClient()
 
type Customer(firstName, lastName) =
        inherit TableEntity(PartitionKey=lastName,RowKey=firstName)    
        member val Email = "" with get, set
        member val PhoneNumber = "" with get, set
        new () = Customer(null, null) 

let customers = tableClient.GetTableReference("Customers")
customers.DeleteIfExists()
customers.CreateIfNotExists()

customers.Execute(TableOperation.Insert(new Customer(firstName="Joe", lastName="Blogs", Email="2323", PhoneNumber="dsf")))
customers.Execute(TableOperation.Insert(new Customer(firstName="Mad", lastName="Max", Email="2323", PhoneNumber="dsf")))
customers.Execute(TableOperation.Insert(new Customer(firstName="Jessica", lastName="Simpson", Email="jessica@test.com", PhoneNumber="dddfg")))
customers.Execute(TableOperation.Insert(new Customer(firstName="Bill", lastName="Gates", Email="sdfsdf", PhoneNumber="dsf")))

(new TableQuery<Customer>())
|> customers.ExecuteQuery
|> Seq.iter(fun c -> printfn "Customer (%s %s, %s, %s)" c.PartitionKey c.RowKey c.Email c.PhoneNumber) 