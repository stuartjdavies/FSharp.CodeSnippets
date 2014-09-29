open System

// Composition
let f(x : Int32) = x * x
let g(x : Int32) = x + x 
let h1 = f >> g
printf "h(10) = %d" (h1(10)) 

// Associativity Rule
let h x = (x + 2)
printfn "(f . g) h = f . ( g . h) -> (Left=%d, Right=%d)" 
                                (((f >> g) >> (h))(5))  
                                (((f) >> (g >> h))(5)) 

//Identity function 
let identity x = x
let a = 10
printfn "The identity(a) which equals 10 is %d" (identity(10)) 

// Monoids
// Binary operation and Identity Element
let Sum xs = xs |> List.reduce (+)
printfn "Sum Total - %d" (Sum([1;2;3;4]))

let Multiply xs = xs |> List.reduce (*)
printfn "Multiply Total - %d" (Sum([1;2;3;4]))

// Workflows (Influenced from monads) 
type MaybeBuilder() =
    member this.Bind(x, f) = Option.bind f x
    member this.Return(x) = Some x
    member this.ReturnFrom(x) = x

let Maybe = new MaybeBuilder()
let SomeComputation(x) = Some(x)

let result = Maybe {
                let! x = SomeComputation(10) 
                return x;
             }

match result with
| Some x -> printfn "Result - %d" x 
| None -> printfn "Result - None"

