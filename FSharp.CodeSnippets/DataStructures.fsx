open System

// Build Different sequences

// Infinite seq
Seq.initInfinite(fun n -> n * n) |> Seq.take 30 |> Seq.iter(fun x -> printf "%d " x)
printfn ""

Seq.initInfinite(fun n -> n * n) |> Seq.nth 5 |> (fun x -> printfn "Element %d" x) 
printfn ""

// Arithmetic Sequences
//Seq.initInfinite(fun n -> n * n) |> Seq.take 30 |> Seq.iter(fun x -> printf "%d " x)
//printfn ""

// Fib seq
Seq.unfold (fun (m,n) -> Some (m, (n,n+m))) (0,1) |> Seq.take 30 |> Seq.iter(fun x -> printf "%d " x)
printfn ""

[|"Jan";"Feb";"Mar";"Apr";"May"; "Jun";"Jul";"Aug";"Sep";"Oct";"Nov";"Dec"|]
|> Seq.pairwise |> Seq.map(fun (a,b) -> System.String.Format("{0}-{1}", a,b)) 
|> Seq.iter(fun r -> printfn "%s" r) 

[|"Jan";"Feb";"Mar";"Apr";"May"; "Jun";"Jul";"Aug";"Sep";"Oct";"Nov";"Dec"|]
|> Seq.pairwise |> Seq.map(fun (a,b) -> System.String.Format("{0}-{1}", a,b)) 
|> Seq.iter(fun r -> printfn "%s" r) 

[|1..4|] |> Seq.collect(fun i -> [| for n in 1 .. i -> n * n |]) |> Seq.iter(fun e -> printf "%d " e)
printfn ""

Seq.unfold(fun state -> if (state > 10) then None else Some(state, state + 1)) 0 |> Seq.iter(fun e -> printf "%d " e)

Seq.unfold(fun state -> if (snd(state) > 10) then None else Some(fst(state), (fst(state) + 1, snd(state) + fst(state) + 1))) (0,0) 
|> Seq.iter(fun e -> printf "%d " e)


let fib = Seq.unfold (fun state ->
    if (snd state > 1000) then None
    else Some(fst state + snd state, (snd state, fst state + snd state))) (1,1)
printfn "\nThe sequence fib contains Fibonacci numbers." 
for x in fib do printf "%d " x

type Test = {
        A : string;
        C : string;
}
    
let b = [| { Test.A="TestB"; C="aaa" } ; { Test.A="TestB"; C="bbb" }; { Test.A="TestB"; C="ccc" } |] |> Seq.distinctBy(fun x -> x.A) 

[| "Stuart"; "Davies" ; "Davies" |] |> Seq.distinct

Seq.init 10 (fun x -> x) |> Seq.iter(fun x -> printfn "%d " x)  

let ds = (dict [ "A", 1; "B", 2])

ds.["A"]

#load "../packages/FsLab.0.0.19/FsLab.fsx"

open FSharp.Charting

Seq.zip ["A";"B";"C";"D"] [1;2;3;4] |> Chart.Bar




