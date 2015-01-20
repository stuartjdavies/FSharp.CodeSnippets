﻿#load "../packages/FsLab.0.0.19/FsLab.fsx"

//#r @"FSharp.Data.TypeProviders.dll" 
#load "../packages/XPlot.GoogleCharts.1.0.1/XPlot.GoogleCharts.fsx"
#I "../packages/HtmlAgilityPack.1.4.9/lib/Net45/"
#r "HtmlAgilityPack.dll"
 
open System
open System.IO
open System.Linq
open System.Collections.Generic
open System.Net
open FSharp.Data
open FSharp.Charting
//open XPlot.GoogleCharts
open System.Text.RegularExpressions
open HtmlAgilityPack



let downloadPageAsString(url : string)= (new WebClient()).DownloadString url         
                  
let getAnchorsOnPage url = let doc = new HtmlDocument()
                           downloadPageAsString url |> doc.LoadHtml 
                           doc.DocumentNode.Descendants().Where(fun x -> String.Compare(x.Name,"a", true) = 0)

let getYears() = getAnchorsOnPage """http://www.aemo.com.au/Electricity/Data/Price-and-Demand/Aggregated-Price-and-Demand-Data-Files"""
                 |> Seq.filter(fun anchor -> let success, num = Int16.TryParse(anchor.InnerText)
                                             success)
                 |> Seq.map(fun anchor -> (Int32.Parse(anchor.InnerText.Trim()), String.Format("http://www.aemo.com.au{0}", anchor.GetAttributeValue("href", null).Trim())))

let getMonthFromFileName(url : string)  = try
                                            Int32.Parse(url.Substring(url.LastIndexOf("_") - 2, 2))
                                          with
                                          | ex -> 0 
                                           
let getMonths (yearUrls : (int * string) seq) = 
                            yearUrls |> Seq.map(fun (year, url) -> getAnchorsOnPage url 
                                                                   |> Seq.filter(fun anchor -> anchor.Attributes.Contains("href") = true && 
                                                                                                  anchor.GetAttributeValue("href", null).Contains(".csv") && 
                                                                                                  anchor.GetAttributeValue("href", null).Contains((year.ToString())))
                                                                   |> Seq.map(fun anchor -> (year, getMonthFromFileName(anchor.GetAttributeValue("href", null).Trim()), 
                                                                                                    anchor.InnerText, anchor.GetAttributeValue("href", null).Trim())))                                                                   
                                      |> Seq.concat                            

// Can't figure out why the CsvProvider is not working
// type PriceAndDemandSchema = CsvProvider<"""./Data/DATA201411_VIC1.csv""">                                                                                                                                 
// let getPriceAndDemands (urls : string seq) = urls |> Seq.map(fun url -> use wc = new WebClient()
//                                                                        let csv = wc.DownloadString(url).Trim() 
//                                                                        PriceAndDemandSchema.Parse(csv).Rows)
                                                                                                                           
                                                                                   
let getPriceAndDemands (url : string) = let wc = new WebClient()
                                        let s = wc.DownloadString(url).Trim()
                                        s.Split '\r' |> Seq.map(fun ln -> let fields = ln.Split(',')
                                                                          (fields.[0], fields.[1], fields.[2], fields.[3], fields.[4]))

let sortMonthsDesc months = months |> Seq.sortBy (fun (y, m, _, _) -> -((y * 10) + m))
let sortMonthsAsc months = months |> Seq.sortBy (fun (y, m, _, _) -> ((y * 10) + m))
let filterByState state months = months |> Seq.filter (fun (_, _, s, _) -> s = state )

getYears() |> getMonths |> Seq.length

#time
getYears() |> getMonths |> sortMonthsDesc |> filterByState "NSW" |> Seq.take 24 
           |> Seq.map(fun (_, _, _, url) -> getPriceAndDemands(url)) 
           |> Seq.concat                      
           |> Seq.map(fun (_, dt, demand, rrp, _) -> (dt, rrp))             
           |> Seq.toList |> List.rev
           |> Chart.Line    
#time

