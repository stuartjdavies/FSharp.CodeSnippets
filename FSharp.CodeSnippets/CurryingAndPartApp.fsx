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
printfn ""

