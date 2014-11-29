#r @"..\packages\WindowsAzure.ServiceBus.2.4.6.0\lib\net40-full\Microsoft.ServiceBus.dll" 
#r "System.Runtime.Serialization.dll"
 
open System
open System.IO
open System.Linq
open Microsoft.ServiceBus
open Microsoft.ServiceBus.Messaging

let connectionString = ""
let queueName = ""

let namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

let CreateQueueIfNotExists qName = if (namespaceManager.QueueExists(qName) = false) then
                                        namespaceManager.CreateQueue(qName) |> ignore                                        
                                        true
                                   else
                                        false                                        

CreateQueueIfNotExists queueName

let client = QueueClient.CreateFromConnectionString(connectionString, queueName)

seq { 0 .. 20 } |> Seq.iter(fun n -> client.Send(new BrokeredMessage(String.Format("Message - {0}", n))))

client.ReceiveBatch(40) |> Seq.iter(fun m -> try
                                               printfn "Recieved message %s" (m.GetBody<string>())
                                               m.Complete()
                                             with
                                             | e -> printfn "Exception receiving message - %s" e.Message 
                                                    m.Abandon())
                                                     
                                                            



