#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r "Microsoft.Office.Interop.Excel.dll"

open System
open RDotNet
open RProvider
open RProvider.``base``
open RProvider.graphics
open RProvider.stats

// construct the data vectors using c()
let xdata = [-2.0;-1.64;-1.33;-0.7;0.0;0.45;1.2;1.64;2.32;2.9]
let ydata = [0.699369;0.700462;0.695354;1.03905;1.97389;2.41143;1.91091;0.919576;-0.730975;-1.42001]


let ds = namedParams [
            "xdata", box xdata;
            "ydata", box ydata;
         ] |> R.data_frame

// look at it
R.plot(ds)

// some starting values
let p1 = 1.0
let p2 = 0.2

// do the fit
// start=[p1=p1;p2=p2]
let fit = R.nls(formula="ydata ~ p1*cos(p2*xdata) + p2*sin(p1*xdata)", data=ds)

// summarise
R.summary(fit)

//let df = R.data_frame(xdata = R.seq(R.min(xdata),R.max(xdata),R.len=200))
//lines(new$xdata,predict(fit,newdata=new))
