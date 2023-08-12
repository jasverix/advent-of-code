module Y2022.Tests.Day14

open NUnit.Framework
open Y2022.Day14

[<Test>]
let ``Get coordinates between`` () =
    Assert.AreEqual("(498, 6) (497, 6) (496, 6)", (498, 6) |> coordinatesBetween (496, 6) |> Y2022.List.toString)

    Assert.AreEqual(
        "(498, 6) (498, 7) (498, 8) (498, 9) (498, 10)",
        (498, 6) |> coordinatesBetween (498, 10) |> Y2022.List.toString
    )

[<Test>]
let ``Rock path`` () =
    Assert.AreEqual(
        "(498, 4) (498, 5) (498, 6) (497, 6) (496, 6)",
        "498,4 -> 498,6 -> 496,6" |> toPath |> toCoordinates |> Y2022.List.toString
    )

let testInput = "498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9
"

[<Test>]
let ``Run until sand is in abyss``() =
    let rockMap = testInput |> toRockMap false
    let result, _ = getResult (fun _ _ -> ()) rockMap
    
    Assert.AreEqual(24, result |> Set.count)

[<Test>]
let ``Run until sand is blocking``() =
    let rockMap = testInput |> toRockMap true
    Assert.IsTrue(rockMap.FloorPosition.IsSome)
    Assert.AreEqual(11, rockMap.FloorPosition.Value)
    let result, _ = getResult (fun _ _ -> ()) rockMap
    
    Assert.AreEqual(93, result |> Set.count)
