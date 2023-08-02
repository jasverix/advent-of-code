module Y2022.Tests.DayTwo

open NUnit.Framework
open Y2022.DayTwo

let testInput = "A Y
B X
C Z"

[<Test>]
let ``Get total score``() =
    Assert.AreEqual(15, testInput |> getTotalScore1)
    Assert.AreEqual(12, testInput |> getTotalScore2)