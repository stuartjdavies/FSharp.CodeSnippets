#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r "../packages/FSharp.Management.0.1.1/lib/net40/FSharp.Management.dll"
#r "../packages\FSharp.Management.0.1.1\lib/net40/FSharp.Management.PowerShell.dll"
#r "System.Management.Automation"
#r "Microsoft.Management.Infrastructure"

open FSharp.Management
open Microsoft.PowerShell
open FSharp.Charting
open System.Linq
open System.Drawing

type PS = PowerShellProvider< PSSnapIns="WDeploySnapin3.0", Is64BitRequired=false>

let PrintItems(result) = match (result) with
                         | Choice1Of2 x -> x |> Seq.iter(fun item ->  printfn "%s" (item.ToString()))
                         | _ -> printfn "Null result"

let toOption(c) = match(c) with
                  | Choice1Of2 x -> Some(x)
                  | _ -> None

PS.``Set-ExecutionPolicy``(executionPolicy=ExecutionPolicy.RemoteSigned) 

let events = (PS.``Get-EventLog``("application") |> (fun el -> match(el) with
                                                               | Choice2Of4 log -> Some(log)
                                                               | _ -> None)).Value
              
query {
     for e in events do
     groupBy(new System.DateTime(e.TimeWritten.Year, e.TimeWritten.Month, e.TimeWritten.Day)) into g
     select(g.Key, g.Count())
} 
|> Chart.Line
|> Chart.WithTitle("Events logged per day")

let GetCategoryColor(c) = match(c) with
                          | "Warning" -> Color.Orange
                          | "Error" -> Color.Red
                          | "Information" -> Color.LightBlue
                          | "FailureAudit" -> Color.Green
                          | _ -> Color.Black

events |> Seq.filter(fun e -> e.EntryType.ToString() <> "0")
       |> Seq.groupBy(fun e -> e.EntryType.ToString())
       |> Seq.map(fun (et, e) -> let totalPerDays =  e |> Seq.groupBy(fun sge -> new System.DateTime(sge.TimeWritten.Year, sge.TimeWritten.Month, sge.TimeWritten.Day))
                                                               |> Seq.map(fun (date, dates) -> (date, (dates |> Seq.length)))
                                 (et, totalPerDays))
                                     
       |> Seq.map(fun (et, totalsPerDay) -> Chart.Line(Name=et, Color=GetCategoryColor(et), data=totalsPerDay))
       |> Chart.Combine
       |> Chart.WithLegend(Enabled=true)
       |> Chart.WithTitle("Entry types logged per day")
