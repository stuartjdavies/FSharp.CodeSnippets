open System

// Volume, milliliters.
[<Measure>] type ml 

// Volume, liters.
[<Measure>] type L

// Grams
[<Measure>] type g

let mlPerLiter : float<ml/L> = 1000.0<ml/L>
let convertMlToLitres(x : float<ml>) = x / mlPerLiter
let smallBottle = 500.00<ml>

// Show compile errors
// let testConversionError = 500.00<ml> + 200.00<g>
printfn "Two small bottles of milk equal %f Litres." (convertMlToLitres(smallBottle * 3.0)/1.0<L>)



