#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r @"FSharp.Data.TypeProviders.dll" 
#load "../packages/XPlot.GoogleCharts.1.0.1/XPlot.GoogleCharts.fsx"

open System.IO
open System.Drawing
open System.Windows.Forms
open System.Linq
open System
open XPlot.GoogleCharts

let startPath= """C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\"""

let rec getDirectories path =
         seq {                
                yield path
                for subDir in Directory.GetDirectories(path) do 
                    yield! getDirectories(subDir)                                                                                              
         }  

         
let getFiles paths  = 
        seq {
            for path in paths do 
                yield! Directory.GetFiles path 
        }        
                 
let filterOutStrings stringsToFilter  (xs : string seq) = xs |> Seq.filter(fun d -> (stringsToFilter |> Seq.exists(fun s -> d.Contains(s) ) = false))            
                          
let toFileInfo files = files |> Seq.map(fun f -> FileInfo(f)) 

let files = getDirectories startPath 
            |> filterOutStrings ["\\packages"; "\\lib"; "\\bin"; "\\obj"; "\.git"; "\.nuget"; "\\Properties";"\\data";"\\Microsoft.WindowsAzure.Caching"] 
            |> getFiles 
            |> Seq.filter (fun file -> file.Contains(".fsx"))

let countDatesPerDay (xs : System.DateTime seq) = xs |> Seq.groupBy(fun x -> DateTime(x.Year, x.Month, x.Day)) 
                                                     |> Seq.map(fun (g, dts) -> (g, (dts|> Seq.length)))

let countDatesPerMonth (xs : System.DateTime seq) = xs |> Seq.groupBy(fun x -> DateTime(x.Year, x.Month, 1)) 
                                                       |> Seq.map(fun (g, dts) -> (g, (dts |> Seq.length)))
                                                                                                              
let sortDatesAsc (xs : System.DateTime seq) = xs |> Seq.sortBy(fun x -> x.ToFileTimeUtc())
let sortDatesDesc (xs : System.DateTime seq) = xs |> Seq.sortBy(fun x -> -x.ToFileTimeUtc())

printfn "Number of FSharp scripts = %d" (files |> Seq.length)
//printfn "Lines of FSharp code = %d" (files )

files |> toFileInfo 
      |> Seq.map(fun fi -> fi.LastWriteTime) 
      |> sortDatesAsc 
      |> countDatesPerDay
      |> Seq.skip 1   
      |> Chart.Bar
      |> Chart.Show                             

files |> toFileInfo 
      |> Seq.map(fun fi -> fi.CreationTime) 
      |> sortDatesAsc 
      |> countDatesPerDay   
      |> Chart.Bar
      |> Chart.Show 
      
files |> toFileInfo 
      |> Seq.map(fun fi -> fi.CreationTime) 
      |> sortDatesDesc 
      |> countDatesPerMonth  
      |> Chart.Bar
      |> Chart.Show                                                    