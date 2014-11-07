#r @"..\packages\WindowsAzure.Storage.4.3.0\lib\net40\Microsoft.WindowsAzure.Storage.dll" 
 
open System
open System.IO
open System.Linq
open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Queue

//let accountName = ""
//let accountKey = ""
//let url =  ""

let accountName = "testaccountstu"
let accountKey = "U4d4wijY3tqQHNVXYBhmkrpMNjX0zDT9910nV4FYu4ABCLOM9QPjva2uSui96yHuzq707zCAFE2yL99xuy8GPw=="
let url =  "https://testaccountstu.queue.core.windows.net/"

let credentials = new Auth.StorageCredentials(accountName, accountKey)
let storageAccount = new CloudStorageAccount(credentials, true)
storageAccount.QueueEndpoint = Uri(url)
let queueClient = storageAccount.CreateCloudQueueClient()

// Retrieve a reference to a queue.
let queue = queueClient.GetQueueReference("myqueue");

// Create the queue if it doesn't already exist.
queue.CreateIfNotExists()

// Create a message and add it to the queue.
seq { 1 .. 10 } |> Seq.iter (fun n -> let message = new CloudQueueMessage(content=String.Format("Message {0}", n))
                                      queue.AddMessage(message))

// Get the queue length
queue.FetchAttributes()
printfn "Number of messages in queue: %d" queue.ApproximateMessageCount.Value

// Display all messages in queue
queue.GetMessages(20) |> Seq.iter(fun m -> printfn "Message contents - %s" (m.AsString))

// Peek at the next message
queue.PeekMessage() |> (fun m -> printfn "Message Contents - %s" (m.AsString))

// Change contents of a queued message
queue.GetMessage() |> (fun m -> m.SetMessageContent("Updated contents.") 
                                queue.UpdateMessage(m, TimeSpan.FromSeconds(0.0), (MessageUpdateFields.Content ||| MessageUpdateFields.Visibility)))

// De-queue the next messageHow to: De-queue the next message
queue.GetMessage() |> (fun m -> printfn "Retrieved message %s" (m.AsString)
                                queue.DeleteMessage(m)
                                printfn "Deleted message %s" (m.AsString))

// Process all messages in less than 5 minutes, deleting each message after processing.
queue.GetMessages(20, visibilityTimeout=new Nullable<TimeSpan>(TimeSpan.FromMinutes(5.0))) |> Seq.iter(fun m -> printfn "Deleting messages %s" (m.AsString)
                                                                                                                queue.DeleteMessage(m));

// Delete a queue 
queue.Delete()
