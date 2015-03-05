#load "DynamoDB.fsx"

open System
open System.Collections.Generic
open Amazon.S3
open Amazon.S3.Model
open Amazon.Runtime
open Amazon.SecurityToken
open Amazon.CloudTrail
open Setup
open AmazonS3
open DynamoDB


//let cloudTrailClient = new AmazonCloudTrailClient(awsAccessKeyId, 
//                                                  awsSecretAccessKey, 
//                                                  Amazon.RegionEndpoint.APSoutheast2)

