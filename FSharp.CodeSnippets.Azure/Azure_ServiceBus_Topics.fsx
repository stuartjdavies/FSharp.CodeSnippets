#r @"..\packages\WindowsAzure.ServiceBus.2.4.6.0\lib\net40-full\Microsoft.ServiceBus.dll" 
#r "System.Runtime.Serialization.dll"
 
open System
open System.IO
open System.Linq
open Microsoft.ServiceBus
open Microsoft.ServiceBus.Messaging
open System.Collections.Generic

let connectionString = @"Endpoint=sb://stuarttestservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=mhub6BPxFjKiBT9ovNBlbln5SD2U2MyeeUsZJ0lCZWo="

let namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

let CreateTopicIfNotExists t = if (namespaceManager.TopicExists(t) = false) then
                                 namespaceManager.CreateTopic(t)                                
                               else
                                 namespaceManager.GetTopic(t)

let CreateSubIfNotExists t s = if (namespaceManager.SubscriptionExists(t, s) = false) then
                                 namespaceManager.CreateSubscription(t,s)                                                                
                               else
                                 namespaceManager.GetSubscription(t,s)

let CreateSubWithFilterIfNotExists t s f = if (namespaceManager.SubscriptionExists(t,s) = false) then
                                             namespaceManager.CreateSubscription(topicPath=t, name=s, filter=f)                                                                              
                                           else
                                             namespaceManager.GetSubscription(t, s)

let ProcessMessages(ms : IEnumerable<BrokeredMessage>) = ms |> Seq.iter(fun m -> try
                                                                                    printfn "Recieved message %s" (m.GetBody<string>())
                                                                                    m.Complete() |> ignore
                                                                                  with
                                                                                  | e -> printfn "Exception receiving message - %s" e.Message 
                                                                                         m.Abandon())
                                 
let topicDesc = CreateTopicIfNotExists "Topic1"
let sub1Desc = CreateSubIfNotExists topicDesc.Path "Sub1"
let sub2Desc = CreateSubIfNotExists topicDesc.Path "Sub2"
let sub3Desc = CreateSubWithFilterIfNotExists "Topic1" "Sub3" (new SqlFilter("MessagePriorty <= 3") :> Filter)

// Publish Messages
TopicClient.CreateFromConnectionString(connectionString, topicDesc.Path) |> (fun client -> seq { 0 .. 20 } |> Seq.iter(fun n -> client.Send(new BrokeredMessage(String.Format("Message - {0}", n)))))

// Publish High priority Messages
TopicClient.CreateFromConnectionString(connectionString, topicDesc.Path) 
|> (fun client -> seq { 21 .. 40 } 
                  |> Seq.iter(fun n -> let m = new BrokeredMessage(String.Format("Message - {0}", n))
                                       m.Properties.["MessagePriorty"] <- 1 
                                       client.Send(m)))

// Retrieve messages for Sub1
printfn "Processing Topic1, Sub1 messages"
SubscriptionClient.CreateFromConnectionString(connectionString, topicDesc.Path, sub1Desc.Name).ReceiveBatch(40) |> ProcessMessages

printfn "Processing Topic1, Sub2 messages"
SubscriptionClient.CreateFromConnectionString(connectionString, topicDesc.Path, sub2Desc.Name).ReceiveBatch(40) |> ProcessMessages

printfn "Processing Topic1, Sub3 messages"
SubscriptionClient.CreateFromConnectionString(connectionString, topicDesc.Path, sub3Desc.Name).ReceiveBatch(40) |> ProcessMessages

// Delete Subscriptions
namespaceManager.DeleteSubscription("Topic1", "Sub1")
namespaceManager.DeleteSubscription("Topic1", "Sub2")
namespaceManager.DeleteSubscription("Topic1", "Sub3")

// Delete Topic
namespaceManager.DeleteTopic "TestTopic"