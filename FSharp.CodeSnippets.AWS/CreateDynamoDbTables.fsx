#load "CloudTrail.fsx"

open DynamoDB
open Setup
open Amazon.DynamoDBv2;
open Amazon.DynamoDBv2.DocumentModel
open Amazon.DynamoDBv2.Model

let dynamoDbClient= createDynamoDbClientFromCsvFile("""c:\AWS\Stuart.Credentials.csv""")



createTable { DynamoDBTableSchema.TableName = "PurchaseOrders";
              Columns = Map [ ("Id", ScalarTypeString)]
              PrimaryKey = Hash("Id");  
              ProvisionedCapacity=Standard;
              GlobalSecondaryIndexes = Set.empty
              LocalSecondaryIndexes = Set.empty } dynamoDbClient

// Insert data 

createTable { DynamoDBTableSchema.TableName = "SalesOrders";
              Columns = Map [ ("Id", ScalarTypeString); ("DateSold", ScalarTypeString) ]                            
              PrimaryKey = HashAndRange("Id", "DateSold");                                  
              ProvisionedCapacity=Standard  
              GlobalSecondaryIndexes = Set.empty
              LocalSecondaryIndexes = Set.empty  } dynamoDbClient

// Insert data 

createTable { DynamoDBTableSchema.TableName = "MusicCollection";
              Columns = Map [ ("Artist", ScalarTypeString); ("SongTitle", ScalarTypeString); ("AlbumTitle", ScalarTypeString) ]                            
              PrimaryKey = HashAndRange("Artist", "SongTitle");                                  
              ProvisionedCapacity=Standard;
              GlobalSecondaryIndexes = Set.empty;
              LocalSecondaryIndexes = Set [ { LocalIndex.Name="AlbumTitleIndex";
                                              Index=HashAndRange("Artist", "AlbumTitle"); 
                                              NonKeyAttributes= Set ["Genre"; "Year"];
                                              ProjectionType=IncludeOnly } ] } dynamoDbClient

// Insert data 

createTable { DynamoDBTableSchema.TableName = "WeatherData";
              Columns = Map [ ("Location", ScalarTypeString); ("Date", ScalarTypeString); ("Precipitation", ScalarTypeNumber) ]                            
              PrimaryKey = HashAndRange("Location", "Date");                                  
              ProvisionedCapacity=Standard;
              GlobalSecondaryIndexes = Set [ { GlobalIndex.Name="AlbumTitleIndex";
                                               Index=HashAndRange("Artist", "AlbumTitle");                                                
                                               ProjectionType=All;
                                               NonKeyAttributes= Set []
                                               ProvisionedCapacity=Standard } ]; 
              LocalSecondaryIndexes = Set.empty } dynamoDbClient

// Insert data 

createTable { DynamoDBTableSchema.TableName = "WeatherData";
              Columns = Map [ ("Location", ScalarTypeString); ("Date", ScalarTypeString); ("Precipitation", ScalarTypeNumber) ]                            
              PrimaryKey = HashAndRange("Location", "Date");                                  
              ProvisionedCapacity=Standard;
              GlobalSecondaryIndexes = Set [ { GlobalIndex.Name="AlbumTitleIndex";
                                               Index=HashAndRange("Artist", "AlbumTitle");                                                
                                               ProjectionType=All;
                                               NonKeyAttributes= Set []
                                               ProvisionedCapacity=Standard } ]; 
              LocalSecondaryIndexes = Set.empty } dynamoDbClient

// Insert data 


deleteTable "MusicCollection" dynamoDbClient
deleteTable "WeatherData" dynamoDbClient
deleteTable "SalesOrders" dynamoDbClient 
deleteTable "PurchaseOrders" dynamoDbClient 

()
(** Save a document **)

