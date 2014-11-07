#r "Microsoft.WindowsAzure.ServiceRuntime.dll"
#r @"..\packages\WindowsAzure.Storage.4.3.0\lib\net40\Microsoft.WindowsAzure.Storage.dll" 
 
open System
open System.IO
open System.Linq
open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Blob

//let accountName = ""
//let accountKey = ""
//let url =  ""

let accountName = "testaccountstu"
let accountKey = "U4d4wijY3tqQHNVXYBhmkrpMNjX0zDT9910nV4FYu4ABCLOM9QPjva2uSui96yHuzq707zCAFE2yL99xuy8GPw=="
let url =  "https://testaccountstu.blob.core.windows.net/"

let credentials = new Auth.StorageCredentials(accountName, accountKey)
let storageAccount = new CloudStorageAccount(credentials, true)
storageAccount.BlobEndpoint = Uri(url)
let blobClient = storageAccount.CreateCloudBlobClient()

let container = blobClient.GetContainerReference("mycontainer")

// container.DeleteIfExists()
container.CreateIfNotExists()

container.SetPermissions(new BlobContainerPermissions(PublicAccess = BlobContainerPublicAccessType.Blob )); 


//
// Upload blobs to a container
//
["SEEK_AU_EI_Data_Sep2014.xls", @"C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.Data\SEEK_AU_EI_Data_Sep2014.xls";
 "AusBeerConsuption.xls", @"C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.Data\AusBeerConsuption.xls"] 
|> Seq.iter (fun (blobName, fileName) -> container.GetBlockBlobReference(blobName) |> (fun blockBlob -> blockBlob.UploadFromFile(fileName, FileMode.Open)))

//
// List blobs in a container
//
container.ListBlobs(null, false) |> Seq.iter(fun b -> match b with
                                                      | :? CloudBlockBlob as blockBlob -> printfn "Block blob of Name %s length %d: %s" blockBlob.Name blockBlob.Properties.Length (blockBlob.Uri.ToString())       
                                                      | :? CloudPageBlob as pageBlob -> printfn "Page blob of length %d: %s" pageBlob.Properties.Length (pageBlob.Uri.ToString())  
                                                      | :? CloudBlobDirectory as dirBlob -> printfn "Directory: %s" (dirBlob.Uri.ToString())
                                                      | _ -> printfn "Unknown Blob type")

//
// Download blobs in a container 
//
container.ListBlobs(null, false) |> Seq.iter(fun b -> match b with
                                                      | :? CloudBlockBlob as blockBlob -> printfn "Writing blog %s" blockBlob.Name
                                                                                          blockBlob.DownloadToFile(String.Format("C:\\temp\\{0}", blockBlob.Name), FileMode.CreateNew)        
                                                      | _ -> printfn "Unknown Blob type")

//
// Remove blobs in a container
//
container.ListBlobs(null, false) |> Seq.iter(fun b -> match b with
                                                      | :? CloudBlockBlob as blockBlob -> blockBlob.Delete()       
                                                      | :? CloudPageBlob as pageBlob -> pageBlob.Delete()        
                                                      | :? CloudBlobDirectory as dirBlob -> printfn "Can't seem to remove directory blobs"
                                                      | _ -> printfn "Unknown Blob type")
