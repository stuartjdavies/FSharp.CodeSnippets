#load "../packages/FsLab.0.0.19/FsLab.fsx"
 
open FSharp.Data
open RProvider
open RProvider.``base``
open Deedle
open Deedle.RPlugin
open RProvider.Internal.Converters
 
let wb = WorldBankData.GetDataContext()
let countries = wb.Countries
 
let pop2000 = series [ for c in countries -> c.Code => c.Indicators.``Population, total``.[2000]]
let pop2013 = series [ for c in countries -> c.Code => c.Indicators.``Population, total``.[2013]]
let surface = series [ for c in countries -> c.Code => c.Indicators.``Surface area (sq. km)``.[2013]]
 
let df = frame [ "Pop2000" => pop2000; "Pop2013" => pop2013; "Surface" => surface ]
df?Codes <- df.RowKeys
 
open RProvider.rworldmap 

let map = R.joinCountryData2Map(df,"ISO3","Codes")
R.mapCountryData(map,"Pop2000") 

df?Density <- df?Pop2013 / df?Surface
df?Growth <- (df?Pop2013 - df?Pop2000) / df?Pop2000
 
let map2 = R.joinCountryData2Map(df,"ISO3","Codes")
R.mapCountryData(map2,"Density")
R.mapCountryData(map2,"Growth")
