#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r @"FSharp.Data.TypeProviders.dll" 
#load "../packages/XPlot.GoogleCharts.1.0.1/XPlot.GoogleCharts.fsx"


 
open System
open System.IO
open System.Linq
open System.Collections.Generic
open System.Net
open FSharp.Data
open XPlot.GoogleCharts
                                             
type seekAddsPage = JsonProvider<"""C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.DataScience\data\SeekSearch.json""">

let buildSeekUrl keywords (page : int)= 
        //Azure+AWS        
        """https://api.seek.com.au/v2/jobs/search?&callback=jQuery18209120329900179058_1421128856139&keywords=""" + keywords + """&hirerId=&hirerGroup=&page=""" + page.ToString() + """&classification=&subclassification=&graduateSearch=false&displaySuburb=&suburb=&location=&nation=3000&area=&isAreaUnspecified=false&worktype=&salaryRange=0-999999&salaryType=annual&dateRange=999&sortMode=KeywordRelevance&engineConfig=&usersessionid=akwv3vskkkuyif4zhzz0w3m2&userid=&userqueryid=26411845208856486&include=expanded&_=1421128856489"""

let getSeekPageAddJson (url : string) =
        let wc = new WebClient()
        let response = wc.DownloadString(url) 
        let json = response.Remove(response.Length - 1).Remove(0, response.IndexOf("(") + 1)
        seekAddsPage.Parse(json)

let getSeekAdJson keywords page = buildSeekUrl keywords page |> getSeekPageAddJson 

let getSeekAds keywords  = 
       let rec aux(keywords, page) =
                seq {
                     let json = (getSeekAdJson keywords page)                  
                     let rows = json.Data
                     let numberOfRows = json.Data.Length

                     if (numberOfRows > 0) then
                       yield! rows
                       if (numberOfRows <> json.TotalCount) then                            
                         yield! aux(keywords, (page + 1))
                     else
                       Seq.empty
                }                                                             
       aux(keywords, 1) 

let getJobCount (xs : string seq) = xs |> Seq.map(fun x-> let totalAdCount = getSeekAdJson x 1 |> (fun x -> x.TotalCount)
                                                          (x.Replace("F%23", "F#"), totalAdCount))                                                               

let getJobCountByLocation(xs : seekAddsPage.Datum seq) = xs |> Seq.groupBy(fun x -> x.Location) 
                                                            |> Seq.map(fun (x, ys) -> (x, ys |> Seq.length))
                                                       

["Haskell";"F%23";"Scala";"Lisp";"Erlang";"Clojure";"Erlang"] |> getJobCount |> Chart.Bar |> Chart.WithTitle "Seek jobs mentioning functional programing on seek" |> Chart.Show
["Haskell";"F%23";"Scala";"Lisp";"Erlang";"Clojure";"Erlang"] |> getJobCount |> Seq.sumBy(fun (_,x) -> x)

["Azure";"AWS"] |> getJobCount |> Chart.Bar |> Chart.Show
["Azure";"AWS"] |> getJobCount |> Chart.Pie |> Chart.WithTitle "Azure vs AWS on Seek" |> Chart.Show

["Big Data";"R";"Hadoop"; "Spark"; "Pig"; "Hive"; "Python"; "Tableau"; "Machine Learning"] |> getJobCount |> Chart.Bar |> Chart.Show
["C#"; "Javascript"; "Java"; "Scala"; "VB.NET"; "Fortran"; "Cobol"; "F%23"; "Haskell"] |> getJobCount |> Chart.Bar |> Chart.Show
["Prince"; "Agile"] |> getJobCount |> Chart.Bar |> Chart.Show
["Postgres"; "Sql Server"; "Oracle"; "MongoDB"; "CouchDB"; "Cassandra"; "NoSQL"] |> getJobCount |> Chart.Bar |> Chart.Show
["Spanish"; "Italian"; "French"; "Japanese"; "Chinese"; "Arabic"; "Russian"] |> getJobCount |> Chart.Bar |> Chart.Show
["BizTalk"] |> getJobCount

"Italian" |> getSeekAds  |> getJobCountByLocation |> Chart.Bar |> Chart.Show

"Italian" |> getSeekAds |> Seq.filter (fun x -> x.Title.Contains("Sales")) 
                        |> Seq.filter (fun x -> x.Location.Contains("Melbourne"))                         
                        |> Seq.iter(fun x -> printfn "%d\r %s\r %s" x.Id x.Title x.Teaser) 



"AWS" |> getSeekAds |> getJobCountByLocation |> Chart.Bar |> Chart.Show
"AWS" |> getSeekAds |> getJobCountByLocation |> Chart.Pie |> Chart.Show

"AWS" |> getSeekAds |> getJobCountByLocation |> Chart.Pie |> Chart.Show
"Azure" |> getSeekAds |> getJobCountByLocation |> Chart.Bar |> Chart.Show
"Scala" |> getSeekAds |> getJobCountByLocation |> Chart.Bar |> Chart.Show
"Clojure" |> getSeekAds |> getJobCountByLocation |> Chart.Bar |> Chart.Show
"BizTalk" |> getSeekAds |> getJobCountByLocation |> Chart.Bar |> Chart.Show
"Italian" |> getSeekAds  |> getJobCountByLocation |> Seq.sortBy(fun (_, l) -> -l) |> Seq.iter(fun (x,y) -> printfn "%s %d" x y) 
"Spanish" |> getSeekAds |> getJobCountByLocation |> Seq.sortBy(fun (_, l) -> -l) |> Seq.iter(fun (x,y) -> printfn "%s %d" x y)
"C#" |> getSeekAds |> Seq.take 300 |> getJobCountByLocation |> Chart.Bar |> Chart.Show
 