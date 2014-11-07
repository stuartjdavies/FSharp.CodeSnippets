#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r "System.Data.dll"
#r "FSharp.Data.TypeProviders.dll"
#r "System.Data.Linq.dll"
#r "System.ServiceModel.dll"
#r "System.Xml.Linq.dll"

open System
open System.Data
open System.Data.Linq
open Microsoft.FSharp.Data.TypeProviders
open Microsoft.FSharp.Linq
open FSharp.Data

// This service was unreliable at the time of writting so it may need a kick
type GlobalWeatherService = WsdlService<"http://www.webservicex.com/globalweather.asmx?WSDL">
type GlobalWeatherResultSchema = XmlProvider<"""<CurrentWeather>
                                                    <Location>Melbourne Airport, Australia (YMML) 37-40S 144-50E 141M</Location>
                                                    <Time>Sep 29, 2014 - 09:30 PM EDT / 2014.09.30 0130 UTC</Time>
                                                    <Wind> from the N (350 degrees) at 24 MPH (21 KT) gusting to 36 MPH (31 KT):0</Wind>
                                                    <Visibility> greater than 7 mile(s):0</Visibility>
                                                    <Temperature> 68 F (20 C)</Temperature>
                                                    <DewPoint> 44 F (7 C)</DewPoint>
                                                    <RelativeHumidity> 42%</RelativeHumidity>
                                                    <Pressure> 29.71 in. Hg (1006 hPa)</Pressure>
                                                    <Status>Success</Status>
                                               </CurrentWeather>""">

let globalWeatherClient = GlobalWeatherService.GetGlobalWeatherSoap()
let rawResult = globalWeatherClient.GetWeather("Melbourne", "Australia")
let result = GlobalWeatherResultSchema.Parse(rawResult.Replace("""<?xml version="1.0" encoding="utf-16"?>""", String.Empty).Trim())

printfn "Weather Stats" 
printfn "Time - %s" result.Time
printfn "Location - %s" result.Location
printfn "Temperature - %s" result.Temperature
