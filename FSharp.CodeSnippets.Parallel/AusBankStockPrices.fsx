#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r "../packages/FSharp.Collections.ParallelSeq.1.0.2/lib/net40/FSharp.Collections.ParallelSeq.dll"

open System.Net
open Microsoft.FSharp.Control.WebExtensions
open FSharp.Data
open FSharp.Charting
open FSharp.Collections.ParallelSeq

let stocks = [ ("Westpac", "http://ichart.finance.yahoo.com/table.csv?s=WBC.AX"); 
               ("Commonwealth Bank", "http://ichart.finance.yahoo.com/table.csv?s=CBA.AX"); 
               ("ANZ", "http://ichart.finance.yahoo.com/table.csv?s=ANZ.AX");
               ("Bendigo And Adelaide Bank Ltd", "http://ichart.finance.yahoo.com/table.csv?s=BEN.AX");
               ("Macquarie Group Limited", "http://ichart.finance.yahoo.com/table.csv?s=MQG.AX")]

type StockSchema = CsvProvider<"http://ichart.finance.yahoo.com/table.csv?s=WBC.AX">               

let GetExecTime(f : (unit -> unit)) =
        let stopwatch = System.Diagnostics.Stopwatch.StartNew()
        f()
        stopwatch.Stop()
        stopwatch.Elapsed

let seqTime = GetExecTime (fun () -> stocks |> Seq.map (fun (name,url) -> (name, Http.RequestString(url)))
                                            |> Seq.map (fun (name, data) -> let stock = StockSchema.Parse(data)
                                                                            (name, [ for row in stock.Rows -> row.Date, row.Open]))
                                            |> Seq.iter(fun (name, _) -> printfn "Seq Downlaoded %s" name))


let asyncTime = GetExecTime (fun () -> seq { for s in stocks -> 
                                                async { return (fst(s), ([for row in StockSchema.Parse(Http.RequestString(snd(s))).Rows -> row.Date, row.Open]))}}
                                       |> Async.Parallel
                                       |> Async.RunSynchronously
                                       |> Seq.iter(fun (name, _) -> printfn "Async Downlaoded %s" name))                       

let pseqTime = GetExecTime (fun () -> stocks |> PSeq.map (fun (name,url) -> (name, Http.RequestString(url)))
                                             |> PSeq.map (fun (name, data) -> let stock = StockSchema.Parse(data)
                                                                              (name, [ for row in stock.Rows -> row.Date, row.Open]))
                                             |> Seq.iter(fun (name, _) -> printfn "PSeq Downlaoded %s" name))

[("PSeq", pseqTime.TotalMilliseconds);
 ("Seq", seqTime.TotalMilliseconds);
 ("Async", asyncTime.TotalMilliseconds)]
|> Chart.Bar

