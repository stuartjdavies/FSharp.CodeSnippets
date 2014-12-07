#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r "../packages/FSharp.Collections.ParallelSeq.1.0.2/lib/net40/FSharp.Collections.ParallelSeq.dll"

open System.Net
open Microsoft.FSharp.Control.WebExtensions
open FSharp.Data
open FSharp.Charting
open FSharp.Collections.ParallelSeq
open System.Collections.Generic;
open System.Collections.Concurrent;
open System.Diagnostics;
open System.Linq;
open System.Threading;
open System.Threading.Tasks;
open System.Linq

// Influenced by Phil Trelfords article - Seq vs Streams.

let GetExecTime(f : (unit -> unit)) =
        let stopwatch = System.Diagnostics.Stopwatch.StartNew()
        f()
        stopwatch.Stop()
        stopwatch.Elapsed
 
let pseqTime = GetExecTime (fun () -> let total = [| 1L .. 1000000L |] |> PSeq.reduce (+)
                                      printfn "PSeq total %d" total)  

let seqTime = GetExecTime (fun () -> let total = [| 1L .. 1000000L |] |> Seq.reduce (+)
                                     printfn "Seq total %d" total)  

let arrayTime = GetExecTime (fun () -> let total = [| 1L .. 1000000L |] |> Array.reduce (+)
                                       printfn "PSeq total %d" total)  

let plinqTime = GetExecTime (fun () -> let total = ([| 1L .. 1000000L |] ).AsParallel().Sum()                                         
                                       printfn "PLinq total %d" total)  

let linqTime = GetExecTime (fun () -> let total = ([| 1L .. 1000000L |]).Sum()                                        
                                      printfn "Linq total %d" total)  


[("PSeq", pseqTime.TotalMilliseconds);
 ("Array", seqTime.TotalMilliseconds);
 ("PLinq", plinqTime.TotalMilliseconds);
 ("linq", plinqTime.TotalMilliseconds);
 ("Seq", seqTime.TotalMilliseconds)] |> Chart.Bar