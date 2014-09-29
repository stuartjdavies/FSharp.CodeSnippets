let Add x y = x + y
let Inc = Add 1 
let IncList = List.map Inc
printf "The incremented list is "
(IncList [1;2;3;4]) |> List.iter (fun x -> printf "%d " x)
printfn ""


