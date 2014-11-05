#r "../packages/AWSSDK.2.3.5.0/lib/net45/AWSSDK.dll"

open System;
open System.Collections.Generic;
open Amazon.DynamoDBv2;
open Amazon.DynamoDBv2.Model;
open Amazon.Runtime;
open Amazon.Util;

let accessKeyId=""
let secretAccessKeyID=""

let client = new AmazonDynamoDBClient(accessKeyId,secretAccessKeyID,Amazon.RegionEndpoint.APSoutheast2)  

let PrintItem(attributeList : Dictionary<string, AttributeValue>) =
        attributeList |> Seq.iter(fun kvp -> let attributeName = kvp.Key;
                                             let value = kvp.Value

                                             printfn "%s %s %s %s %s"
                                                    attributeName

                                                    (if (value.S = null) then
                                                       String.Empty
                                                    else
                                                       String.Format("S=[{0}]", value.S))

                                                    (if (value.N = null) then
                                                       String.Empty
                                                    else
                                                       String.Format("S=[{0}]", value.N))
                                                    
                                                    (if (value.SS = null) then
                                                       String.Empty
                                                    else
                                                       String.Format("SS=[{0}]", String.Join(",", value.SS.ToArray())))
                                                    
                                                    (if (value.NS = null) then
                                                       String.Empty
                                                    else
                                                       String.Format("NS=[{0}]", String.Join(",", value.NS.ToArray()))))                                                                                                 
                 
        printfn "************************************************" |> ignore                      

let GetBook(id, tableName) = new GetItemRequest(TableName = tableName, 
                                                Key = Dictionary<string, AttributeValue>(dict [ ("Id", new AttributeValue(N = id.ToString())) ]),
                                                ReturnConsumedCapacity = (ReturnConsumedCapacity.op_Implicit "TOTAL") ) |> client.GetItem         
            
let FindRepliesInLast15DaysWithConfig(forumName, threadSubject) =                                                                              
        let rec aux(lastKeyEvaluated : Dictionary<string, AttributeValue>, acc) =    
                    let replyId = String.Format("{0}#{1}", forumName, threadSubject)    

                    let twoWeeksAgoDate = DateTime.UtcNow - TimeSpan.FromDays(15.0);
                    let twoWeeksAgoString = twoWeeksAgoDate.ToString(AWSSDKUtils.ISO8601DateFormat)
                        
                    let request = new QueryRequest(TableName="Reply",                                           
                                                   KeyConditions = Dictionary<string, Condition>(
                                                                            dict [ "Id", new Condition(ComparisonOperator=ComparisonOperator.op_Implicit"EQ",
                                                                                               AttributeValueList = List ([| new AttributeValue(S = replyId) |]));                                                                                              
                                                                                   "ReplyDateTime", new Condition(ComparisonOperator=ComparisonOperator.op_Implicit"GT",
                                                                                                        AttributeValueList = List [| new AttributeValue(S = twoWeeksAgoString) |]) ]),                                                                         
                                                   
                                                   ProjectionExpression="Id, ReplyDateTime, PostedBy", // Optional parameter.
                                                   ConsistentRead = true, // Optional parameter.
                                                   Limit = 2, // The Reply table has only a few sample items. So the page size is smaller.
                                                   ExclusiveStartKey = lastKeyEvaluated,
                                                   ReturnConsumedCapacity = ReturnConsumedCapacity.op_Implicit "TOTAL")
                    
                    let response = client.Query(request)
                    let lastKeyEvaluated = response.LastEvaluatedKey
                                                                  
                    if (lastKeyEvaluated = null || lastKeyEvaluated.Count = 0) then
                      (response.Items |> Seq.toList) @ acc                        
                    else                                                
                      let items = response.Items |> Seq.toList                                                                        
                      aux(lastKeyEvaluated, items @ acc) 
        aux(null, List.Empty) |> List.rev                     
                                                                      
let gbResponse = GetBook(101, "ProductCatalog")
gbResponse.Item |> PrintItem
printfn "No. of reads used (by get book item) %f\n" gbResponse.ConsumedCapacity.CapacityUnits        
             
// Query - Get replies posted in the last 15 days for a forum thread.
let forumName="Amazon DynamoDB"
let threadSubject="DynamoDB Thread 1"

FindRepliesInLast15DaysWithConfig(forumName, threadSubject) |> Seq.iter(fun item -> item |> PrintItem)
             
 