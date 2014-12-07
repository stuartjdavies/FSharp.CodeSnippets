#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r "../packages/FSharp.Collections.ParallelSeq.1.0.2/lib/net40/FSharp.Collections.ParallelSeq.dll"

open System.IO
open FSharp.Collections.ParallelSeq
open FSharp.Charting
open System.Linq

let rec GetFiles(path) =                                                            
          ((Directory.GetFiles(path) |> Array.toSeq), (Directory.GetDirectories(path) |> Seq.collect(fun d -> GetFiles(d)))) 
          ||> Seq.append 

let rec GetFilesPSeq(path) =                                                                      
          ((Directory.GetFiles(path) |> Array.toSeq), (Directory.GetDirectories(path) |> PSeq.collect(fun d -> GetFilesPSeq(d)))) 
          ||> PSeq.append
          
let rec GetFilesArray(path) =                                                                      
          ((Directory.GetFiles(path)), (Directory.GetDirectories(path) |> Array.collect(fun d -> GetFilesArray(d)))) 
          ||> Array.append
  
              
let path="""C:\Program Files (x86)"""

let GetExecTime(f : (unit -> unit)) =
        let stopwatch = System.Diagnostics.Stopwatch.StartNew()
        f()
        stopwatch.Stop()
        stopwatch.Elapsed

let seqTime = GetExecTime (fun () -> GetFiles(path)
                                     |> Seq.filter(fun fileName -> FileInfo(fileName).Length > 100000000L)
                                     |> Seq.iter(fun fileName -> printfn "FileName - %s Bytes - %d"  fileName (FileInfo(fileName).Length)))

let pseqTime = GetExecTime (fun () -> GetFiles(path)
                                      |> PSeq.filter(fun fileName -> FileInfo(fileName).Length > 100000000L)
                                      |> PSeq.iter(fun fileName -> printfn "FileName - %s Bytes - %d"  fileName (FileInfo(fileName).Length)))

let arrayTime = GetExecTime (fun () -> GetFilesArray(path)
                                       |> Array.filter(fun fileName -> FileInfo(fileName).Length > 100000000L)
                                       |> Array.iter(fun fileName -> printfn "FileName - %s Bytes - %d"  fileName (FileInfo(fileName).Length)))

let plinqTime = GetExecTime (fun () -> GetFilesArray(path).AsParallel()
                                                          .Where(fun fileName -> FileInfo(fileName).Length > 100000000L)
                                                          .ToList().ForEach(fun fileName -> printfn "FileName - %s Bytes - %d"  fileName (FileInfo(fileName).Length)))

let linqTime = GetExecTime (fun () -> GetFilesArray(path).Where(fun fileName -> FileInfo(fileName).Length > 100000000L)
                                                         .ToList().ForEach(fun fileName -> printfn "FileName - %s Bytes - %d"  fileName (FileInfo(fileName).Length)))


[("PSeq", pseqTime.TotalMilliseconds);
 ("Seq", seqTime.TotalMilliseconds);
 ("Array", arrayTime.TotalMilliseconds);
 ("PLinq", plinqTime.TotalMilliseconds);
 ("Linq", plinqTime.TotalMilliseconds)]
|> Chart.Bar