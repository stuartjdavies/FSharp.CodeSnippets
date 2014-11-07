#r "../packages/FSharp.Data.2.0.9/lib/net40/FSharp.Data.dll"
#r "../packages/FSharp.Data.Toolbox.Twitter.0.2.1/lib/net40/FSharp.Data.Toolbox.Twitter.dll"
open FSharp.Data.Toolbox.Twitter
open FSharp.Data

let key = "aEDlMt8fgOGMnmuLRpM7rGKWs"
let secret = "sIFAjNPJvMtmwP8w8C4AEgKEbhcF3zZfCyqHg6v05XSeGnjm63"

let twitter = Twitter.AuthenticateAppOnly(key, secret)

//let connector = Twitter.Authenticate(key, secret) 
//
// Run this part after you obtain PIN
// let twitter = connector.Connect("7808652")
//
// Get a list of ID numbers of friends and followers 
// for the current signed-in user
// (requires full authentication)
//let friends = twitter.Connections.FriendsIds()
//let followers = twitter.Connections.FollowerIds()
//printfn "Number of friends: %d" (friends.Ids |> Seq.length)
//printfn "Number of followers: %d" (followers.Ids |> Seq.length)
//
// Get a list IDs of friends and followers for a specific user 
//let followersFSorg = twitter.Connections.FriendsIds(userId=880772426L)
//let friendsFSorg = twitter.Connections.FollowerIds(screenName="fsharporg")
//
// Get information about connection between specific users
//let fs = twitter.Connections.Friendship(880772426L, 94144339L)
//fs.Relationship.Source.ScreenName
//fs.Relationship.Target.ScreenName
//fs.Relationship.Source.Following
//fs.Relationship.Source.FollowedBy
