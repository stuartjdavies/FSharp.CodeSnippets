#r "../packages/FSharp.Data.2.0.15/lib/net40/FSharp.Data.dll"
#r "../packages/FSharp.Data.Toolbox.Twitter.0.2.1/lib/net40/FSharp.Data.Toolbox.Twitter.dll"
open FSharp.Data.Toolbox.Twitter
open FSharp.Data

let key = "aEDlMt8fgOGMnmuLRpM7rGKWs"
let secret = "sIFAjNPJvMtmwP8w8C4AEgKEbhcF3zZfCyqHg6v05XSeGnjm63"

let twitter = Twitter.Authenticate(key, secret)
