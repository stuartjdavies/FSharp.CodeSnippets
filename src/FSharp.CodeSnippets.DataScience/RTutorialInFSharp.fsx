#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r "Microsoft.Office.Interop.Excel.dll"

//
// This is rewritten version in F# from the tuturial
// http://msenux.redwoods.edu/math/R/
// 
open System
open RDotNet
open RProvider
open RProvider.``base``
open RProvider.graphics
open RProvider.stats
open MathNet


//
// Section: Simple plot in R
//
let xs = seq { 0.0 .. 0.01 .. 2.00 }
let ys = seq { for x in xs -> 2.0*sin(2.0*Math.PI*(x - 1.0/4.0)) }

let df = namedParams ["xs",xs;"ys",ys;] |> R.data_frame 

// Example 1
R.plot(df)

// Example 2
R.plot(namedParams ["x", box xs; "y", box ys; "type", box "l"])

// Example 3 
R.plot(namedParams ["x", box xs; "y", box ys;"xlab",box "x-axis";" ylab", box "y-axis"; "type", box "l"])

//
// Section: Pie Charts in R
//

// Example 1
let mypie=[40;30;20;10]
R.pie(mypie)

// Example 2
R.names(mypie) = R.list(["Red";"Blue";"Green";"Red"])
R.pie(mypie)

// Example 3
let mycolors=["red";"blue";"green";"brown"]
R.pie(mypie,col=mycolors)

//
// Section: Bar charts in R
//

// Example 1
let x=[40;30;20;10]
R.barplot(x)

// Example 2
R.names(x) = R.list(["Red";"Blue";"Green";"Brown"])
R.barplot_default(x)

// Example 3
R.barplot_default(x,col=["red";"blue";"green";"brown"])

//
// Section: Linear Regression
//
let ages = seq { 18 .. 29 }
let heights = [76.1;77.0;78.1;78.2;78.8;79.7;79.9;81.1;81.2;81.8;82.8;83.5]

printfn "Number of ages - %d" (Seq.length(ages))
printfn "Number of heights - %d" (Seq.length(heights))

let ds = namedParams [
            "height", box heights;
            "age", box ages;
         ] |> R.data_frame

R.plot ds
let result=R.lm(formula="age~height", data=ds)
R.abline result


