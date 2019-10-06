module Reproduction

open System
open Amazon.CDK
open Amazon.CDK.AWS.S3
open Amazon.CDK.AWS.S3.Deployment

type ReproductionStack (parent: Construct, id: string, props: IStackProps) as this =
    inherit Stack (parent, id, props)
    let webAppRoot = "./dist"

    let bucket = Bucket (this, "MyBucket", BucketProps (WebsiteIndexDocument = "index.html"))
    let bucketDeploymentProps =
        BucketDeploymentProps (
            Sources = [| Source.Asset webAppRoot |],
            DestinationKeyPrefix = "web/",
            DestinationBucket = bucket,
            RetainOnDelete = (false |> Nullable))
    let bucketDeployment = BucketDeployment (this, "MyDeployment", bucketDeploymentProps) |> ignore

[<EntryPoint>]
let main argv =
    let app = App (AppProps ())
    let reproStack = ReproductionStack (app, "ReproductionStack", StackProps ())
    app.Synth () |> ignore
    0 // return an integer exit code
