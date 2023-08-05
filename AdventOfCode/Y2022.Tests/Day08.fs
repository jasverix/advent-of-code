module Y2022.Tests.Day08

open NUnit.Framework
open Y2022.Day08

let testInput =
    "30373
25512
65332
33549
35390
"

[<Test>]
let ``Get map`` () =
    let map = testInput |> toMap

    Assert.AreEqual(3, map[(2, 2)])
    Assert.AreEqual(5, map[(2, 3)])
    Assert.AreEqual(5, map[(1, 2)])

[<Test>]
let Walk () =
    let map = testInput |> toMap

    let walk =
        map
        |> getTree (1, 0)
        |> walk South
        |> Seq.map (fun t -> t.Height.ToString())
        |> String.concat " "

    Assert.AreEqual("5 5 3 5", walk)

[<Test>]
let ``Get all visible trees``() =
    let allVisibleTrees = testInput |> toMap |> allVisibleTrees |> Array.ofSeq
    Assert.AreEqual(21, allVisibleTrees |> Array.length)

[<Test>]
let ``Scenic score``() =
    let map = testInput |> toMap
    let tree = map |> getTree (2,3) |> Option.get
    Assert.AreEqual(1, tree |> viewDistance South)
    Assert.AreEqual(2, tree |> viewDistance North)

[<Test>]
let ``Get best scenic score``() =
    let allScenicScores = testInput |> toMap |> allScenicScores
    Assert.AreEqual(8, allScenicScores |> Seq.max)