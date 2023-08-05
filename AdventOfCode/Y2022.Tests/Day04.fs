module Y2022.Tests.Day04

open NUnit.Framework
open Y2022.Day04

let testInput = "2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8
"

[<Test>]
let ``Is overlapping works``() =
    Assert.IsTrue("1-4, 2-3" |> toAssignmentPair |> isTotallyOverlapping)
    Assert.IsFalse("1-4, 6-8" |> toAssignmentPair |> isTotallyOverlapping)
    
[<Test>]
let ``Count overlapping``() =
    Assert.AreEqual(2, testInput |> toAssignmentPairs |> countTotallyOverlapping)
    
[<Test>]
let ``Count intersecting``() =
    Assert.AreEqual(4, testInput |> toAssignmentPairs |> countIntersecting)