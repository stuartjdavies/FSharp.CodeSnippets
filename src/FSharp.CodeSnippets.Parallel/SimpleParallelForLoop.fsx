//
// Based on the example, O rewrote in F#
// http://msdn.microsoft.com/en-us/library/dd460713(v=vs.110).aspx
// 
// Not a great functional example but anyway it increases spead.
//

open System;
open System.Collections.Generic;
open System.Collections.Concurrent;
open System.Diagnostics;
open System.Linq;
open System.Threading;
open System.Threading.Tasks;


Parallel.ForEach((seq { 0 .. 20 }), (fun x -> (printfn "Item i - %d" x)))

// Example
let InitializeMatrix(rows, cols) =
        let r = new Random()
        Array2D.init rows cols (fun _ _ -> Convert.ToDouble(r.Next()))               
                              
let MultiplyMatricesSequential(matA : double [,], matB : double [,], result : double [,]) = 
        for i in 0 .. matA.GetLength(0) - 1 do
           for j in 0 .. matB.GetLength(1) - 1 do 
              for k in 0 .. matA.GetLength(1) - 1 do                   
                  (result.[i,j] <- result.[i,j] + matA.[i,k] * matB.[k,j]) |> ignore                  
                            
let MultiplyMatricesParallel(matA :  double [,],  matB :  double [,], result :  double [,]) =  
        Parallel.For(0, (matA.GetLength(0) - 1), fun i -> for j in 0 .. matB.GetLength(1) - 1 do 
                                                             let mutable temp = 0.0 // For speed
                                                             for k in 0 .. matA.GetLength(1) - 1  do
                                                                (temp <- temp + matA.[i,k] * matB.[k,j]) |> ignore                                                           
                                                                (result.[i,j] <- temp) |> ignore)

let colCount = 180 * 2
let rowCount = 2000 * 2
let colCount2 = 270 * 2
let m1 = InitializeMatrix(rowCount, colCount)
let m2 = InitializeMatrix(colCount, colCount2)
let mutable result1 = Array2D.zeroCreate<double> rowCount colCount2

// First do the sequential version.
printfn "Executing sequential loop..."
let stopwatch = new Stopwatch()
stopwatch.Start();

MultiplyMatricesSequential(m1, m2, result1)
stopwatch.Stop();
let time1 = stopwatch.ElapsedMilliseconds
stopwatch.Reset()

// Do the parallel loop.
printfn "Executing parallel loop..."
stopwatch.Start()
let mutable result2 = Array2D.zeroCreate<double> rowCount colCount2
MultiplyMatricesParallel(m1, m2, result2)
stopwatch.Stop()
let time2 = stopwatch.ElapsedMilliseconds

printfn "Sequential loop time in milliseconds: %d" time1
printfn "Parallel loop time in milliseconds: %d" time2