module SeqTests

#if !FABLE_COMPILER

open Expecto
open SampleDomain
open TestData
open TestHelpers
open System
open FsToolkit.ErrorHandling

let traverseResultTests =
  testList "Seq.traverseResultM Tests" [
    testCase "traverseResultM with a seq of valid data" <| fun _ ->
      let tweets = [ "Hi"; "Hello"; "Hola" ] |> List.toSeq
      let expected = Seq.map tweet tweets |> List.ofSeq
      let actual = Seq.traverseResultM Tweet.TryCreate tweets |> Result.defaultWith (fun _ -> failwith "") |> List.ofSeq
      Expect.equal actual expected "Should have a list of valid tweets"

    testCase "traverseResultM with few invalid data" <| fun _ ->
      let tweets = [""; "Hello"; aLongerInvalidTweet] |> Seq.ofList
      let actual = Seq.traverseResultM Tweet.TryCreate tweets
      Expect.equal actual (Error emptyTweetErrMsg) "traverse the seq and return the first error"
  ]

let sequenceResultMTests =
  testList "Seq.sequenceResultM Tests" [
    testCase "traverseResultM with a seq of valid data" <| fun _ ->
      let tweets = [ "Hi"; "Hello"; "Hola" ] |> List.toSeq
      let expected = Seq.map tweet tweets |> Seq.toList
      let actual = Seq.sequenceResultM (Seq.map Tweet.TryCreate tweets)  |> Result.defaultWith(fun _ -> failwith "") |> Seq.toList
      Expect.equal actual expected "Should have a seq of valid tweets"

    testCase "sequenceResultM with few invalid data" <| fun _ ->
      let tweets = [""; "Hello"; aLongerInvalidTweet]
      let actual = Seq.sequenceResultM (Seq.map Tweet.TryCreate tweets) 
      Expect.equal actual (Error emptyTweetErrMsg) "traverse the list and return the first error"
  ]

let traverseResultATests =
  testList "Seq.traverseResultA Tests" [
    testCase "traverseResultA with a seq of valid data" <| fun _ ->
      let tweets = [ "Hi"; "Hello"; "Hola" ] |> List.toSeq
      let expected = Seq.map tweet tweets |> Seq.toList
      let actual = Seq.traverseResultA Tweet.TryCreate tweets |> Result.defaultWith (fun _ -> failwith "") |> Seq.toList
      Expect.equal actual expected "Should have a list of valid tweets"

    testCase "traverseResultA with few invalid data" <| fun _ ->
      let tweets = [ ""; "Hello"; aLongerInvalidTweet ] |> List.toSeq
      let actual = Seq.traverseResultA Tweet.TryCreate tweets
      Expect.equal actual (Error [emptyTweetErrMsg;longerTweetErrMsg]) "traverse the list and return all the errors"
  ]


let sequenceResultATests =
  testList "Seq.sequenceResultA Tests" [
    testCase "traverseResultA with a seq of valid data" <| fun _ ->
      let tweets = ["Hi"; "Hello"; "Hola"] |> List.toSeq
      let expected = Seq.map tweet tweets |> List.ofSeq
      let actual =
          Seq.sequenceResultA (Seq.map Tweet.TryCreate tweets)
          |> Result.defaultWith(fun _ -> failwith "")
          |> Seq.toList
      Expect.equal actual expected "Should have a list of valid tweets"

    testCase "sequenceResultA with few invalid data" <| fun _ ->
      let tweets = [""; "Hello"; aLongerInvalidTweet] |> Seq.ofList
      let actual = Seq.sequenceResultA (Seq.map Tweet.TryCreate tweets) 
      Expect.equal actual (Error [emptyTweetErrMsg;longerTweetErrMsg]) "traverse the list and return all the errors"
  ]

let allTests = testList "Seq Tests" [
  traverseResultTests
  sequenceResultMTests
  traverseResultATests
  sequenceResultATests
]
#endif