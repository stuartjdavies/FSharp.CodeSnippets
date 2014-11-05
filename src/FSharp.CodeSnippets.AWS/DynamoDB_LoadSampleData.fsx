#r "../packages/AWSSDK.2.3.5.0/lib/net45/AWSSDK.dll"

open System
open System.Collections.Generic
open Amazon.DynamoDBv2
open Amazon.DynamoDBv2.DocumentModel
open Amazon.Runtime
open Amazon.SecurityToken

let accessKeyId=""
let secretAccessKeyID=""

let client = new AmazonDynamoDBClient(accessKeyId,secretAccessKeyID,Amazon.RegionEndpoint.APSoutheast2)  
let productCatalogTable = Table.LoadTable(client, "ProductCatalog")

// ********** Add Books *********************
let book1 = new Document()
book1.["Id"] <- DynamoDBEntry.op_Implicit 101
book1.["Title"] <- DynamoDBEntry.op_Implicit "Book 101 Title"
book1.["ISBN"] <- DynamoDBEntry.op_Implicit "111-1111111111"
book1.["Authors"] <- DynamoDBEntry.op_Implicit [| "Author 1" |]
book1.["Price"] <- DynamoDBEntry.op_Implicit -2 // *** Intentional value. Later used to illustrate scan.
book1.["Dimensions"] <- DynamoDBEntry.op_Implicit "8.5 x 11.0 x 0.5"
book1.["PageCount"] <- DynamoDBEntry.op_Implicit 500
book1.["InPublication"] <- DynamoDBEntry.op_Implicit true
book1.["ProductCategory"] <- DynamoDBEntry.op_Implicit "Book"
productCatalogTable.PutItem(book1)

let book2 = new Document()

book2.["Id"] <- DynamoDBEntry.op_Implicit 102
book2.["Title"] <- DynamoDBEntry.op_Implicit "Book 102 Title"
book2.["ISBN"] <- DynamoDBEntry.op_Implicit "222-2222222222"
book2.["Authors"] <- DynamoDBEntry.op_Implicit [| "Author 1"; "Author 2" |] 
book2.["Price"] <- DynamoDBEntry.op_Implicit 20
book2.["Dimensions"] <- DynamoDBEntry.op_Implicit "8.5 x 11.0 x 0.8"
book2.["PageCount"] <- DynamoDBEntry.op_Implicit 600
book2.["InPublication"] <- DynamoDBEntry.op_Implicit true
book2.["ProductCategory"] <- DynamoDBEntry.op_Implicit "Book"
productCatalogTable.PutItem(book2)

let book3 = new Document()
book3.["Id"] <- DynamoDBEntry.op_Implicit 103
book3.["Title"] <- DynamoDBEntry.op_Implicit "Book 103 Title"
book3.["ISBN"] <- DynamoDBEntry.op_Implicit "333-3333333333"
book3.["Authors"] <- DynamoDBEntry.op_Implicit [| "Author 1"; "Author2"; "Author 3" |] 
book3.["Price"] <- DynamoDBEntry.op_Implicit 2000
book3.["Dimensions"] <- DynamoDBEntry.op_Implicit "8.5 x 11.0 x 1.5"
book3.["PageCount"] <- DynamoDBEntry.op_Implicit 700
book3.["InPublication"] <- DynamoDBEntry.op_Implicit false
book3.["ProductCategory"] <- DynamoDBEntry.op_Implicit "Book"
productCatalogTable.PutItem(book3)

// ************ Add bikes. *******************
let bicycle1 = new Document()
bicycle1.["Id"] <- DynamoDBEntry.op_Implicit 201
bicycle1.["Title"] <- DynamoDBEntry.op_Implicit "18-Bike 201" // size, followed by some title.
bicycle1.["Description"] <- DynamoDBEntry.op_Implicit "201 description"
bicycle1.["BicycleType"] <- DynamoDBEntry.op_Implicit "Road"
bicycle1.["Brand"] <- DynamoDBEntry.op_Implicit "Brand-Company A" // Trek, Specialized.
bicycle1.["Price"] <- DynamoDBEntry.op_Implicit 100
bicycle1.["Gender"] <- DynamoDBEntry.op_Implicit "M"
bicycle1.["Color"] <- DynamoDBEntry.op_Implicit [| "Red"; "Black" |]
bicycle1.["ProductCategory"] <- DynamoDBEntry.op_Implicit "Bike"
productCatalogTable.PutItem(bicycle1)

let bicycle2 = new Document()
bicycle2.["Id"] <- DynamoDBEntry.op_Implicit 202
bicycle2.["Title"] <- DynamoDBEntry.op_Implicit "21-Bike 202Brand-Company A"
bicycle2.["Description"] <- DynamoDBEntry.op_Implicit "202 description"
bicycle2.["BicycleType"] <- DynamoDBEntry.op_Implicit "Road"
bicycle2.["Brand"] <- DynamoDBEntry.op_Implicit ""
bicycle2.["Price"] <- DynamoDBEntry.op_Implicit 200
bicycle2.["Gender"] <- DynamoDBEntry.op_Implicit "M" // Mens.
bicycle2.["Color"] <- DynamoDBEntry.op_Implicit [| "Green"; "Black" |]
bicycle2.["ProductCategory"] <- DynamoDBEntry.op_Implicit "Bicycle"
productCatalogTable.PutItem(bicycle2)

let bicycle3 = new Document()
bicycle3.["Id"] <- DynamoDBEntry.op_Implicit 203
bicycle3.["Title"] <- DynamoDBEntry.op_Implicit "19-Bike 203"
bicycle3.["Description"] <- DynamoDBEntry.op_Implicit "203 description"
bicycle3.["BicycleType"] <- DynamoDBEntry.op_Implicit "Road"
bicycle3.["Brand"] <- DynamoDBEntry.op_Implicit "Brand-Company B"
bicycle3.["Price"] <- DynamoDBEntry.op_Implicit 300
bicycle3.["Gender"] <- DynamoDBEntry.op_Implicit "W"
bicycle3.["Color"] <- DynamoDBEntry.op_Implicit [| "Red"; "Green"; "Black" |]
bicycle3.["ProductCategory"] <- DynamoDBEntry.op_Implicit "Bike"
productCatalogTable.PutItem(bicycle3)

let bicycle4 = new Document()
bicycle4.["Id"] <- DynamoDBEntry.op_Implicit 204
bicycle4.["Title"] <- DynamoDBEntry.op_Implicit "18-Bike 204"
bicycle4.["Description"] <- DynamoDBEntry.op_Implicit "204 description"
bicycle4.["BicycleType"] <- DynamoDBEntry.op_Implicit "Mountain"
bicycle4.["Brand"] <- DynamoDBEntry.op_Implicit "Brand-Company B"
bicycle4.["Price"] <- DynamoDBEntry.op_Implicit 400
bicycle4.["Gender"] <- DynamoDBEntry.op_Implicit "W" // Women.
bicycle4.["Color"] <- DynamoDBEntry.op_Implicit [| "Red" |]
bicycle4.["ProductCategory"] <- DynamoDBEntry.op_Implicit "Bike"
productCatalogTable.PutItem(bicycle4)

let bicycle5 = new Document()
bicycle5.["Id"] <- DynamoDBEntry.op_Implicit 205
bicycle5.["Title"] <- DynamoDBEntry.op_Implicit "20-Title 205"
bicycle4.["Description"] <- DynamoDBEntry.op_Implicit "205 description"
bicycle5.["BicycleType"] <- DynamoDBEntry.op_Implicit "Hybrid"
bicycle5.["Brand"] <- DynamoDBEntry.op_Implicit "Brand-Company C"
bicycle5.["Price"] <- DynamoDBEntry.op_Implicit 500
bicycle5.["Gender"] <- DynamoDBEntry.op_Implicit "B" // Boys.
bicycle5.["Color"] <- DynamoDBEntry.op_Implicit [| "Red"; "Black" |]
bicycle5.["ProductCategory"] <- DynamoDBEntry.op_Implicit "Bike"
productCatalogTable.PutItem(bicycle5)
        

let forumTable = Table.LoadTable(client, "Forum")

let forum1 = new Document()
forum1.["Name"] <- DynamoDBEntry.op_Implicit "Amazon DynamoDB" // PK
forum1.["Category"] <- DynamoDBEntry.op_Implicit "Amazon Web Services"
forum1.["Threads"] <- DynamoDBEntry.op_Implicit 2
forum1.["Messages"] <- DynamoDBEntry.op_Implicit 4
forum1.["Views"] <- DynamoDBEntry.op_Implicit 1000

forumTable.PutItem(forum1)

let forum2 = new Document()
forum2.["Name"] <- DynamoDBEntry.op_Implicit "Amazon S3" // PK
forum2.["Category"] <- DynamoDBEntry.op_Implicit "Amazon Web Services"
forum2.["Threads"] <- DynamoDBEntry.op_Implicit 1

forumTable.PutItem(forum2)
        
let threadTable = Table.LoadTable(client, "Thread")

// Thread 1.
let thread1 = new Document()
thread1.["ForumName"] <- DynamoDBEntry.op_Implicit "Amazon DynamoDB" // Hash attribute.
thread1.["Subject"] <- DynamoDBEntry.op_Implicit "DynamoDB Thread 1" // Range attribute.
thread1.["Message"] <- DynamoDBEntry.op_Implicit "DynamoDB thread 1 message text"
thread1.["LastPostedBy"] <- DynamoDBEntry.op_Implicit "User A"
thread1.["LastPostedDateTime"] <- DynamoDBEntry.op_Implicit (DateTime.UtcNow.Subtract(new TimeSpan(14, 0, 0, 0)))
thread1.["Views"] <- DynamoDBEntry.op_Implicit 0
thread1.["Replies"] <- DynamoDBEntry.op_Implicit 0
thread1.["Answered"] <- DynamoDBEntry.op_Implicit false
thread1.["Tags"] <- DynamoDBEntry.op_Implicit [| "index"; "primarykey"; "table" |]

threadTable.PutItem(thread1)

// Thread 2.
let thread2 = new Document()
thread2.["ForumName"] <- DynamoDBEntry.op_Implicit "Amazon DynamoDB" // Hash attribute.
thread2.["Subject"] <- DynamoDBEntry.op_Implicit "DynamoDB Thread 2" // Range attribute.
thread2.["Message"] <- DynamoDBEntry.op_Implicit "DynamoDB thread 2 message text"
thread2.["LastPostedBy"] <- DynamoDBEntry.op_Implicit "User A"
thread2.["LastPostedDateTime"] <- DynamoDBEntry.op_Implicit (DateTime.UtcNow.Subtract(new TimeSpan(21, 0, 0, 0)))
thread2.["Views"] <- DynamoDBEntry.op_Implicit 0
thread2.["Replies"] <- DynamoDBEntry.op_Implicit 0
thread2.["Answered"] <- DynamoDBEntry.op_Implicit false
thread2.["Tags"] <- DynamoDBEntry.op_Implicit [| "index"; "primarykey"; "rangekey" |]

threadTable.PutItem(thread2)

// Thread 3.
let thread3 = new Document()
thread3.["ForumName"] <- DynamoDBEntry.op_Implicit "Amazon S3" // Hash attribute.
thread3.["Subject"] <- DynamoDBEntry.op_Implicit "S3 Thread 1" // Range attribute.
thread3.["Message"] <- DynamoDBEntry.op_Implicit "S3 thread 3 message text"
thread3.["LastPostedBy"] <- DynamoDBEntry.op_Implicit "User A"
thread3.["LastPostedDateTime"] <- DynamoDBEntry.op_Implicit (DateTime.UtcNow.Subtract(new TimeSpan(7, 0, 0, 0)))
thread3.["Views"] <- DynamoDBEntry.op_Implicit 0
thread3.["Replies"] <- DynamoDBEntry.op_Implicit 0
thread3.["Answered"] <- DynamoDBEntry.op_Implicit false
thread3.["Tags"] <- DynamoDBEntry.op_Implicit [| "largeobjects"; "multipart upload" |]

threadTable.PutItem(thread3)

let replyTable = Table.LoadTable(client, "Reply")

// Reply 1 - thread 1.
let thread1Reply1 = new Document()
thread1Reply1.["Id"] <- DynamoDBEntry.op_Implicit "Amazon DynamoDB#DynamoDB Thread 1" // Hash attribute.
thread1Reply1.["ReplyDateTime"] <- DynamoDBEntry.op_Implicit (DateTime.UtcNow.Subtract(new TimeSpan(21, 0, 0, 0))) // Range attribute.
thread1Reply1.["Message"] <- DynamoDBEntry.op_Implicit "DynamoDB Thread 1 Reply 1 text"
thread1Reply1.["PostedBy"] <- DynamoDBEntry.op_Implicit "User A"

replyTable.PutItem(thread1Reply1)

// Reply 2 - thread 1.
let thread1reply2 = new Document()
thread1reply2.["Id"] <- DynamoDBEntry.op_Implicit "Amazon DynamoDB#DynamoDB Thread 1" // Hash attribute.
thread1reply2.["ReplyDateTime"] <- DynamoDBEntry.op_Implicit (DateTime.UtcNow.Subtract(new TimeSpan(14, 0, 0, 0))) // Range attribute.
thread1reply2.["Message"] <- DynamoDBEntry.op_Implicit "DynamoDB Thread 1 Reply 2 text"
thread1reply2.["PostedBy"] <- DynamoDBEntry.op_Implicit "User B"

replyTable.PutItem(thread1reply2)

// Reply 3 - thread 1.
let thread1Reply3 = new Document()
thread1Reply3.["Id"] <- DynamoDBEntry.op_Implicit "Amazon DynamoDB#DynamoDB Thread 1" // Hash attribute.
thread1Reply3.["ReplyDateTime"] <- DynamoDBEntry.op_Implicit (DateTime.UtcNow.Subtract(new TimeSpan(7, 0, 0, 0))) // Range attribute.
thread1Reply3.["Message"] <- DynamoDBEntry.op_Implicit "DynamoDB Thread 1 Reply 3 text"
thread1Reply3.["PostedBy"] <- DynamoDBEntry.op_Implicit "User B"

replyTable.PutItem(thread1Reply3)

// Reply 1 - thread 2.
let thread2Reply1 = new Document()
thread2Reply1.["Id"] <- DynamoDBEntry.op_Implicit "Amazon DynamoDB#DynamoDB Thread 2" // Hash attribute.
thread2Reply1.["ReplyDateTime"] <- DynamoDBEntry.op_Implicit (DateTime.UtcNow.Subtract(new TimeSpan(7, 0, 0, 0))) // Range attribute.
thread2Reply1.["Message"] <- DynamoDBEntry.op_Implicit "DynamoDB Thread 2 Reply 1 text"
thread2Reply1.["PostedBy"] <- DynamoDBEntry.op_Implicit "User A"


replyTable.PutItem(thread2Reply1)

// Reply 2 - thread 2.
let thread2Reply2 = new Document()
thread2Reply2.["Id"] <- DynamoDBEntry.op_Implicit "Amazon DynamoDB#DynamoDB Thread 2" // Hash attribute.
thread2Reply2.["ReplyDateTime"] <- DynamoDBEntry.op_Implicit (DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0))) // Range attribute.
thread2Reply2.["Message"] <- DynamoDBEntry.op_Implicit "DynamoDB Thread 2 Reply 2 text"
thread2Reply2.["PostedBy"] <- DynamoDBEntry.op_Implicit "User A"

replyTable.PutItem(thread2Reply2)
