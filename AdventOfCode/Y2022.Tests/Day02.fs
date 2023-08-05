module Y2022.Tests.Day02

open NUnit.Framework
open Y2022.Day02

let testInput = "A Y
B X
C Z"

[<Test>]
let ``Get total score``() =
    Assert.AreEqual(15, testInput |> getTotalScore1)
    Assert.AreEqual(12, testInput |> getTotalScore2)