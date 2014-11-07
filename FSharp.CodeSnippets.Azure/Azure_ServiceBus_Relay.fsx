#r @"..\packages\WindowsAzure.ServiceBus.2.4.6.0\lib\net40-full\Microsoft.ServiceBus.dll" 
#r "System.Runtime.Serialization.dll"
#r "System.ServiceModel.dll"
 
open System
open System.IO
open System.Linq
open Microsoft.ServiceBus
open Microsoft.ServiceBus.Messaging
open System.Collections.Generic
open System.ServiceModel

// Underconstruction

let connectionString = @"Endpoint=sb://stuarttestservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=mhub6BPxFjKiBT9ovNBlbln5SD2U2MyeeUsZJ0lCZWo="
let namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString)
let key="mhub6BPxFjKiBT9ovNBlbln5SD2U2MyeeUsZJ0lCZWo="
//let key="Endpoint=sb://stuarttestservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=mhub6BPxFjKiBT9ovNBlbln5SD2U2MyeeUsZJ0lCZWo="
let ns="stuarttestservicebus"

namespaceManager.

[<ServiceContract(Namespace = "urn:ps")>]
type IProblemSolver = 
        [<OperationContract>]
        abstract member AddNumbers: int -> int -> int

type ProblemSolver =
        interface IProblemSolver with 
            member this.AddNumbers a b  = a + b

type IProblemSolverChannel = 
         inherit IProblemSolver 
         inherit IClientChannel 

let sh = new ServiceHost(typeof<ProblemSolver>)

sh.AddServiceEndpoint(typeof<IProblemSolver>, new NetTcpBinding(), "net.tcp://localhost:9358/solver");

sh.AddServiceEndpoint(typeof<IProblemSolver>, 
                        new NetTcpRelayBinding(), ServiceBusEnvironment.CreateServiceUri("sb", ns, "solver"))
                                                  .Behaviors.Add(new TransportClientEndpointBehavior(TokenProvider=TokenProvider.CreateSharedSecretTokenProvider( "owner", key)))

sh.Open()

let cf = new ChannelFactory<IProblemSolverChannel>(new NetTcpRelayBinding(), 
                                                   new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("sb", ns, "solver")));

cf.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior(TokenProvider = TokenProvider.CreateSharedSecretTokenProvider("owner",key)));

let ch = cf.CreateChannel()

printfn "Add 4 + 5 = %d" (ch.AddNumbers 4 5)

sh.Close()


