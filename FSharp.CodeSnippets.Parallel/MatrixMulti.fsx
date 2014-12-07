#load "../packages/FsLab.0.0.19/FsLab.fsx"

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
open FSharp.Charting

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

let GetExecTime(f : (unit -> unit)) =
        let stopwatch = System.Diagnostics.Stopwatch.StartNew()
        f()
        stopwatch.Stop()
        stopwatch.Elapsed

let colCount = 180 * 2
let rowCount = 2000 * 2
let colCount2 = 270 * 2
let m1 = InitializeMatrix(rowCount, colCount)
let m2 = InitializeMatrix(colCount, colCount2)

let mutable result1 = Array2D.zeroCreate<double> rowCount colCount2
let seqTime = GetExecTime (fun () -> MultiplyMatricesSequential(m1, m2, result1))

let mutable result2 = Array2D.zeroCreate<double> rowCount colCount2
let parTime = GetExecTime (fun () -> let r = MultiplyMatricesParallel(m1, m2, result2)
                                     printfn "Completed %s" (r.IsCompleted.ToString()))

[("Using Parallel.For", parTime.TotalMilliseconds);
 ("Non Parallel", seqTime.TotalMilliseconds);]
|> Chart.Bar
