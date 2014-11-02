#r "../packages/AWSSDK.2.3.5.0/lib/net45/AWSSDK.dll"

open System
open System.Collections.Generic
open Amazon.DynamoDBv2
open Amazon.DynamoDBv2.DocumentModel
open Amazon.DynamoDBv2.Model
open Amazon.Runtime
open Amazon.SecurityToken

let accessKeyId=""
let secretAccessKeyID=""

let client = new AmazonDynamoDBClient(accessKeyId,secretAccessKeyID,Amazon.RegionEndpoint.APSoutheast2) 
let tableName = "ExampleTable"; 

let WaitUntilTableReady(tableName) =        
        let rec aux(tblName) =              
                    // Let us wait until table is created. Call DescribeTable.
                    System.Threading.Thread.Sleep(5000) // Wait 5 seconds.
                    try                
                        let res = client.DescribeTable(request=new DescribeTableRequest(TableName=tableName))
                    
                        printf "Table name: %s, status: %s" res.Table.TableName (res.Table.TableStatus.ToString())
                                                                  
                        if (res.Table.TableStatus = TableStatus.ACTIVE) then
                           res.Table.TableStatus
                        else
                          aux(tblName)                                              
                    with
                    // DescribeTable is eventually consistent. So you might
                    // get resource not found. So we handle the potential exception.                    
                    | :? ResourceNotFoundException as re -> aux(tblName)         
        aux(tableName)
                                                     
let CreateExampleTable(tableName) =
        printfn "\n*** Creating table ***"
       
        let request = new CreateTableRequest(AttributeDefinitions = List<AttributeDefinition> [                
                                                new AttributeDefinition(AttributeName = "Id",AttributeType= ScalarAttributeType.op_Implicit "N");
                                                new AttributeDefinition(AttributeName = "ReplyDateTime", AttributeType= ScalarAttributeType.op_Implicit "N")],
                                             KeySchema = List<KeySchemaElement> [new KeySchemaElement(AttributeName = "Id", KeyType = KeyType.op_Implicit "HASH");
                                                                                 new KeySchemaElement(AttributeName = "ReplyDateTime", KeyType = KeyType.op_Implicit "RANGE")],                                             
                                             ProvisionedThroughput = new ProvisionedThroughput(ReadCapacityUnits = (int64 5), WriteCapacityUnits = (int64 6)),
                                             TableName=tableName)

        let response = client.CreateTable(request)

        let tableDescription = response.TableDescription
        printf "%s: %s \t ReadsPerSec: %d \t WritesPerSec: %d"
                            (tableDescription.TableStatus.ToString())
                            tableDescription.TableName
                            tableDescription.ProvisionedThroughput.ReadCapacityUnits
                            tableDescription.ProvisionedThroughput.WriteCapacityUnits

        let status = tableDescription.TableStatus
        printfn "%s - %s" tableName (status.ToString())

        WaitUntilTableReady(tableName)

let GetTableNames() =
        let rec aux(lastTableNameEvaluated, acc) =
                let request = new ListTablesRequest(Limit = 2, ExclusiveStartTableName = lastTableNameEvaluated)                

                let response = client.ListTables(request)                                        
                        
                if (response.LastEvaluatedTableName = null) then
                    acc   
                else                         
                    aux(response.LastEvaluatedTableName, (acc @ (response.TableNames |> List.ofSeq)))                                                  
        aux(null, List.Empty)       

let GetTableInformation(tableName) =  
        new DescribeTableRequest(TableName=tableName) |> client.DescribeTable |> (fun r -> r.Table)

let UpdateExampleTable(tableName) =          
       new UpdateTableRequest(TableName = tableName, 
                              ProvisionedThroughput=new ProvisionedThroughput(ReadCapacityUnits=6,WriteCapacityUnits=7))
       |> client.UpdateTable |> ignore            
       WaitUntilTableReady(tableName)
       
let DeleteTable(tableName) = new DeleteTableRequest(TableName=tableName) |> client.DeleteTable                       
        
printfn "\n*** listing tables ***"   
GetTableNames() |> List.iter(fun tn -> printfn "%s" tn)             

printfn "\n*** Retrieving table information ***"

GetTableInformation(tableName) 
|> (fun description -> printfn "Name: %s" description.TableName
                       printfn "# of items: %d" description.ItemCount
                       printfn "Provision Throughput (reads/sec): %d" description.ProvisionedThroughput.ReadCapacityUnits
                       printfn "Provision Throughput (writes/sec): %d" description.ProvisionedThroughput.WriteCapacityUnits
                       ())

printfn "\n*** Updating table ***"

UpdateExampleTable tableName

printfn "*** Deleting table ***"

DeleteTable tableName

printfn "Table is being deleted..."        
