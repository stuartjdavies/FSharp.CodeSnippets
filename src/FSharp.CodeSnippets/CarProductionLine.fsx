open System
open System.Drawing

type Component(name, price, components) =
    member val Name : String = name with get, set
    member val Price : float = price with get, set
    member val Components : Component list = components with get, set 
   
type Wheel =
       inherit Component
       new () = { inherit Component("Wheel", 400.00, []) } 

type Door =
       inherit Component
       new () = { inherit Component("Door", 400.00, []) }

type CarInterior =
       inherit Component
       new () = { inherit Component("CarInterior", 5000.00, []) }
                
type Car(name, price, components) = 
    inherit Component(name, price, components)
    member val Color : Color Option = None with get, set
    
type BarinaSpark() =
     inherit Car("BarinaSpark", 1000.0, [])
                   
type AssemblyLineProcessResult = | Success of Car                       
                                 | Failure of string
            
type CarBuilder() =
        member this.Bind(x, f) = 
                    match(x) with
                    | Success x -> f(x) 
                    | Failure x -> Failure x                     
        member this.Delay(f) = f()
        member this.Return(x) = Success x

let CarAssemblyLine = new CarBuilder()

let PaintBody(color : Color, c : Car) = c.Color <- Some(color)
                                        Success(c) 

let AddWheels(c : Car) = c.Components <- c.Components @ (List.init 4 (fun n -> new Wheel() :> Component ))
                         Success(c) 

let AddDoors(c : Car) = c.Components <- c.Components @ (List.init 4 (fun n -> new Door() :> Component))
                        Success(c) 

let AddInterior(c : Car) = c.Components <- c.Components @ (List.init 4 (fun n -> new CarInterior() :> Component))
                           Success(c) 

let AddOptionalAccesseries(c : Car) = c.Components <- c.Components @ (List.init 4 (fun n -> new Component("OptionalAssessories",50.0,[])))
                                      Success(c) 

let GetBarinaSparkBody() = Success(new BarinaSpark())
                              
let CreateBarinaSpark(color) = CarAssemblyLine {                             
                                    let! carAfterStage1 = GetBarinaSparkBody()   
                                    let! carAfterStage2 = PaintBody(color, carAfterStage1)    
                                    let! carAfterStage3 = AddInterior(carAfterStage2)                        
                                    let! carAfterStage4 = AddWheels(carAfterStage3)
                                    let! carAfterStage5 = AddDoors(carAfterStage4)
                                    let! carAfterStage6 = AddOptionalAccesseries(carAfterStage5)
                               
                                    return carAfterStage6
                                }

let rec GetCompoents(c : Component) =      
          c.Components |> Seq.fold(fun acc c -> if (List.length(c.Components) > 0)  then
                                                  c::(GetCompoents(c) @ acc)
                                                else
                                                  c::acc) List.Empty

let newBarinaSpark = CreateBarinaSpark(Color.Blue)

let components = match(newBarinaSpark) with
                 | Success car -> Some(GetCompoents (car :> Component))
                 | Failure message -> None

let numberOfComponents = components.Value |> List.length
let totalOfCar = components.Value |> List.sumBy (fun c -> c.Price)

printfn "Price of car is $%.00f" totalOfCar



