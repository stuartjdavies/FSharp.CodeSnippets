// Clonninga  data with new value    
type test = { A : int; B : int ; C : int}
do
    let updateB t b = { t with B = b }
    let newTest = updateB { A = 1; B=2; C=3 } 4
    ()

do
    let mystery arg1 arg2 = 
            let v = arg2 * 2 
            (v, arg1 + "!")
    ()

do
    let sum = 0
    for n in 1 .. 5 do
        let sum = sum + n
        printf "%d," sum
    ()

    let mutable sum = 0
    for n in 1 .. 5 do 
        sum <- sum + n
    printf "%d, " sum
    ()

do 
    let order = ("Test", 1, 2)

    fst order

    let getName (name, price, quality) = name
    getName order

    let name, _, _ = order

    let getName order2 =
          let name, _, _ = order2
        name
    getName order

    match order with
    | name, _, _ -> name
    ()

do
    let point3d = (10, 11, -5)
    match point3d with
    | x, y -> printfn "Point: %d, %d" x y
    | _ -> printfn "Not a point!" 
    ()

do 
    let swap (a,b) = (b,a)
    swap ("Answer", 42)
    ()

do
    let onceAndTwice f input = (f input, f (f input))
    ()


    //'a -> 'b -> 'b * 'b 

do 
    let threeValueOr opt1 opt2 = 
        match opt1, opt2 with
        | Some true, _ -> Some true
        | _, Some true -> Some true
        | Some false, Some false -> Some false
        | _ -> None

    threeValueOr None (Some true)
    threeValueOr None (Some false)
    threeValueOr (Some true) (Some false)
    ()

let n = if (printfn "hi"; true) then 1 else 2

() |> (fun () -> 20) |> (fun x -> x * 10) |> (fun x -> x * 10) 

(float "10.0")

(int (float "20.00"))


//let StandardDev xs u = xs |> Seq.map(fun x -> (x - u) ** 2)
//                          |> 
//                          |> Seq.sum


let inline Add a b = a + b

