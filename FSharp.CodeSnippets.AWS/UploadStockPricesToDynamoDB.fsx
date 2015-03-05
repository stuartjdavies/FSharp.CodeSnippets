#load "CloudTrail.fsx"

open DynamoDB
open Setup
open Amazon.DynamoDBv2;
open Amazon.DynamoDBv2.DocumentModel
open Amazon.DynamoDBv2.Model
open FSharp
open FSharp.Data
open System
open AmazonS3
open Amazon.Util

// 
// Load the microsoft msft data from yahoo 
//
type Stocks = CsvProvider<"C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.AWS\Data\YahooStockPriceSchema.csv">
let msft = Stocks.Load("http://ichart.finance.yahoo.com/table.csv?s=MSFT").Cache()
let msftRows = msft.Rows |> Seq.take 1000 |> Seq.toArray

// Create Amazon Client string 
let dynamoDbClient = createDynamoDbClientFromCsvFile """c:\AWS\Stuart.Credentials.csv"""
let s3client = createS3ClientFromCsvFile """c:\AWS\Stuart.Credentials.csv"""

// 
// Create Dynamo DB Database in the cloud
//
createTable { DynamoDBTableSchema.TableName = "MicrosoftStockPrices";
              Columns = Map [ ("ODate", ScalarTypeString) ]                            
              PrimaryKey = Hash("ODate");                                  
              ProvisionedCapacity=Standard;
              GlobalSecondaryIndexes=Set.empty
              LocalSecondaryIndexes=Set.empty } dynamoDbClient

dynamoDbClient |> waitUntilTableIsCreated "MicrosoftStockPrices" 3000

// 
// Insert Microsoft's stock prices in the DynamoDB NoSql Database
// 
msftRows
|> Array.Parallel.map(fun row -> toDocument [ "ODate", toDynamoDbEntry(row.Date.ToString(AWSSDKUtils.ISO8601DateFormat))
                                              "OpenPrice", toDynamoDbEntry(row.Open) 
                                              "HighPrice", toDynamoDbEntry(row.High)
                                              "LowPrice",  toDynamoDbEntry(row.Low)
                                              "ClosePrice", toDynamoDbEntry(row.Close)
                                              "Volume", toDynamoDbEntry(row.Volume) 
                                              "AdjClose", toDynamoDbEntry(row.``Adj Close``) ])                                                                         
|> uploadToDynamoDB "MicrosoftStockPrices" dynamoDbClient

// 
// Run a query
//
let query = { DynamoDbScan.From="MicrosoftStockPrices";
                           Where=(Between("OpenPrice", 45, 46) <&&> 
                                  Between("ClosePrice", 45, 45.5) <&&>
                                  GreaterThan("AdjClose", 44.8)) }
            
query |> runScan dynamoDbClient
      |> Seq.iteri(fun i item -> printfn "%d. Date - %s, Open - %s, Close - %s, Adj. Close=%s"
                                                i item.["ODate"].S item.["OpenPrice"].N 
                                                  item.["ClosePrice"].N item.["AdjClose"].N)

    
//
// Print the Table Summary
//
let info = getTableInfo "MicrosoftStockPrices" dynamoDbClient 
printfn "Table Summary"
printfn "-------------"
printfn "Name: %s" info.TableName
printfn "# of items: %d" info.ItemCount
printfn "Provision Throughput (reads/sec): %d" info.ProvisionedThroughput.ReadCapacityUnits
printfn "Provision Throughput (writes/sec): %d" info.ProvisionedThroughput.WriteCapacityUnits

//                          
// Run a query on the data 
//                                                            
deleteTable "MicrosoftStockPrices" dynamoDbClient
