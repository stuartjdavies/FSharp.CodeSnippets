#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r @"FSharp.Data.TypeProviders.dll" 
 
open System
open System.IO
open System.Linq
open System.Collections.Generic
open System.Net
open FSharp.Data
open FSharp.Charting
                                             
type seekAddsPage = JsonProvider<"""C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.DataScience\data\SeekSearch.json""">

let BuildSeekUrl keywords (page : int)= 
        //Azure+AWS        
        """https://api.seek.com.au/v2/jobs/search?&callback=jQuery18209120329900179058_1421128856139&keywords=""" + keywords + """&hirerId=&hirerGroup=&page=""" + page.ToString() + """&classification=&subclassification=&graduateSearch=false&displaySuburb=&suburb=&location=&nation=3000&area=&isAreaUnspecified=false&worktype=&salaryRange=0-999999&salaryType=annual&dateRange=999&sortMode=KeywordRelevance&engineConfig=&usersessionid=akwv3vskkkuyif4zhzz0w3m2&userid=&userqueryid=26411845208856486&include=expanded&_=1421128856489"""


let GetSeekPageAddJson (url : string) =
        let wc = new WebClient()
        let response = wc.DownloadString(url) 
        let json = response.Remove(response.Length - 1).Remove(0, response.IndexOf("(") + 1)
        seekAddsPage.Parse(json)

let GetSeekPageJson keywords page = BuildSeekUrl keywords page |> GetSeekPageAddJson 

//let keywords = "F%23"
// "Azure+C#"

let GetSystemNumberOfSeekAdds(keywords) = (GetSeekPageJson keywords 1).TotalCount

let GetAllSeekAdds keywords  = 
       let rec aux(keywords, page) =
                seq {
                     let json = (GetSeekPageJson keywords page)                  
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

let BarGraphSeekSearchCount (xs : string seq) = xs |> Seq.map(fun x-> (x.Replace("F%23", "F#"), GetSystemNumberOfSeekAdds(x)))
                                                   |> Chart.Bar            

let BarGraphSeekSearchCountByLocation (keywords : string) = GetAllSeekAdds keywords                                                          
                                                            |> Seq.groupBy(fun x -> x.Location)
                                                            |> Seq.map(fun (x, ys) -> (x, ys |> Seq.length))
                                                            |> Chart.Bar            


["Haskell";"F%23";"Scala";"Lisp";"Erlang";"Clojure"] |> BarGraphSeekSearchCount
["Azure";"AWS"] |> BarGraphSeekSearchCount
["Big Data";"R";"Hadoop"; "Spark"; "Pig"; "Hive"; "Python"; "Tableau"; "Machine Learning"] |> BarGraphSeekSearchCount
["C#"; "Javascript"; "Java"; "Scala"; "VB.NET"; "Fortran"; "Cobol"; "F%23"; "Haskell"] |> BarGraphSeekSearchCount
["Prince"; "Agile"] |> BarGraphSeekSearchCount
["Sql Server"; "Oracle"; "MongoDB"; "CouchDB"; "Cassandra"; "NoSQL"] |> BarGraphSeekSearchCount
["Spanish"; "Italian"; "French"; "Japanese"; "Chinese"; "Arabic"; "Russian"] |> BarGraphSeekSearchCount

"AWS" |> BarGraphSeekSearchCountByLocation
"Azure" |> BarGraphSeekSearchCountByLocation
"Scala" |> BarGraphSeekSearchCountByLocation
"Clojure" |> BarGraphSeekSearchCountByLocation
"BizTalk" |> BarGraphSeekSearchCountByLocation
"BizTalk" |> GetSystemNumberOfSeekAdds
 
GetAllSeekAdds("Italian") |> Seq.groupBy(fun x -> x.Location)
                          |> Seq.map(fun (x, ys) -> (x, ys |> Seq.length))
                          |> Seq.sortBy(fun (_, l) -> -l)
                          |> Seq.iter(fun (x,y) -> printfn "%s %d" x y)

GetAllSeekAdds("Spanish") |> Seq.groupBy(fun x -> x.Location)
                          |> Seq.map(fun (x, ys) -> (x, ys |> Seq.length))
                          |> Seq.sortBy(fun (_, l) -> -l)
                          |> Seq.iter(fun (x,y) -> printfn "%s %d" x y)


let adds = GetAllSeekAdds "C#" |> Seq.take 100;;
 