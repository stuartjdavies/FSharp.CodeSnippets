#r @"..\packages\WindowsAzure.ServiceBus.2.4.6.0\lib\net40-full\Microsoft.ServiceBus.dll" 
#r "System.Runtime.Serialization.dll"
 
open System
open System.IO
open System.Linq
open Microsoft.ServiceBus
open Microsoft.ServiceBus.Messaging

let connectionString = @"Endpoint=sb://stuarttestservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=mhub6BPxFjKiBT9ovNBlbln5SD2U2MyeeUsZJ0lCZWo="
let topic = "TestTopic"

// Under construction


let namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

let CreateTopicIfNotExists t = if (namespaceManager.TopicExists(t) = false) then
                                 let newTopic = namespaceManager.CreateTopic(t) 
                                 namespaceManager.CreateSubscription(newTopic.Path, "topic1") |> ignore
                                 namespaceManager.CreateSubscription(newTopic.Path, "topic2") |> ignore                                                                  
                                 true
                               else
                                 false                                        


let client = TopicClient.CreateFromConnectionString(connectionString, topic)
seq { 0 .. 20 } |> Seq.iter(fun n -> client.Send(new BrokeredMessage(String.Format("Message - {0}", n))))





client.ReceiveBatch(40) |> Seq.iter(fun m -> try
                                               printfn "Recieved message %s" (m.GetBody<string>())
                                               m.Complete()
                                             with
                                             | e -> printfn "Exception receiving message - %s" e.Message 
                                                    m.Abandon())
