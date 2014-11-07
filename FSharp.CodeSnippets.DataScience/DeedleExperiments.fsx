#load "../packages/FsLab.0.0.19/FsLab.fsx"

open System
open Deedle
open FSharp.Charting

let r = new Random()
let dates = Seq.init 10 (fun n -> DateTime.Now.AddHours((float n)))
let values1 = Seq.init 10 (fun n -> n * 10)
let values2 = Seq.init 10 (fun n -> r.Next(0, 100))
   
let first = Series(dates,values1)
let second = Series(dates,values2)

// Standard
let df1 = Frame(["first";"second"], [first;second])

// The same as previously
let df2 = Frame.ofColumns ["first" => first; "second" => second]

// Transposed - here, rows are "first" and "second" & columns are dates
let df3 = Frame.ofRows ["first" => first; "second" => second]

// Create from individual observations (row * column * value)
let df4 = 
  [ ("Monday", "Tomas", 1.0); ("Tuesday", "Adam", 2.1)
    ("Tuesday", "Tomas", 4.0); ("Wednesday", "Tomas", -5.4) ]
  |> Frame.ofValues

// Assuming we have a record 'Price' and a collection 'values'
type Price = { Day : DateTime; Open : float }
let prices = [ { Day = DateTime.Now; Open = 10.1 }
               { Day = DateTime.Now.AddDays(1.0); Open = 15.1 }
               { Day = DateTime.Now.AddDays(2.0); Open = 9.1 } ]

// Creates a data frame with columns 'Day' and 'Open'
let df5 = Frame.ofRecords prices

