// Some of core functional programming concepts convered are:
// 
// - Lambda functions
// - Higher order functions
// - Currying and Partial Applications 
// - Constructs influenced by Category Theory - Composition, Monoids, Workflows(Monads), 
//   Identity function
// - Combinators 
open System

//
// Lambda functions
//
do
    let foo = fun x -> x * x
    ()

//
// Higher order functions
//
do
    let square n =  n * n
    let applyIt = fun op arg -> op arg
    printfn "SquareIt %d" (applyIt square 2)
    ()

//
// Currying and Partial Application
//
do
    let Add x y = x + y
    let Inc = Add 1 
    let IncList = List.map Inc
    printf "The incremented list is "
    (IncList [1;2;3;4]) |> List.iter (fun x -> printf "%d " x)
    printfn ""

    let FilterGreaterThan2 = (>=) 2
    printf "Filtered list of items > 2 is "
    (([1;2;3;4]) |> List.filter FilterGreaterThan2 
                 |> List.iter (fun x -> printf "%d " x))
    ()

//
// Constructs influenced by Category Theory
//

// 
// Combinators
//
// One description of the word "combinator" is used to describe functions whose 
// result depends only on their parameters.
// Below are examples in the F# framework. 
//
// let (|>) x f = f x             // forward pipe
// let (<|) f x = f x             // reverse pipe
// let (>>) f g x = g (f x)       // forward composition
// let (<<) g f x = g (f x)       // reverse composition

// Composition
do
    let f(x : Int32) = x * x
    let g(x : Int32) = x + x 
    let h1 = f >> g
    printf "h(10) = %d" (h1(10)) 
    
    // Associativity Rule
    let h x = (x + 2)
    printfn "(f . g) h = f . ( g . h) -> (Left=%d, Right=%d)" 
                                        (((f >> g) >> (h))(5))  
                                        (((f) >> (g >> h))(5)) 
    ()

//Identity function 
do
    let identity x = x
    let a = 10
    printfn "The identity(a) which equals 10 is %d" (identity(10)) 
    ()

// Monoids
do
    let Sum xs = xs |> List.reduce (+)
    printfn "Sum Total - %d" (Sum([1;2;3;4]))
    
    let Multiply xs = xs |> List.reduce (*)
    printfn "Multiply Total - %d" (Sum([1;2;3;4]))
    ()

// Workflows (Influenced from monads) 
type MaybeBuilder() =
    member this.Bind(x, f) = Option.bind f x
    member this.Return(x) = Some x
    member this.ReturnFrom(x) = x

let Maybe = new MaybeBuilder()
let SomeComputation(x) = Some(x)

do
    let result = Maybe {
                    let! x = SomeComputation(10) 
                    return x;
                 }

    match result with
    | Some x -> printfn "Result - %d" x 
    | None -> printfn "Result - None"
    ()

// Tuples Aa Arguments
open System
do   
    let s = ("Hello {0}", "World") |> String.Format
    ()