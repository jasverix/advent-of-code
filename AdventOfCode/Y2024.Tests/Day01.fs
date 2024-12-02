module Y2024.Tests.Day01

open NUnit.Framework
open Y2024.Day01

let testInput = "3   4
4   3
2   5
1   3
3   9
3   3
"

[<Test>]
let ``Get total distance`` () =
    Assert.That(
        testInput |> parseInput |> totalDistance,
        Is.EqualTo(11)
    )

[<Test>]
let ``Get similarity score``() =
    Assert.That(
        testInput |> parseInput |> totalSimilarityScore,
        Is.EqualTo(31)
        )