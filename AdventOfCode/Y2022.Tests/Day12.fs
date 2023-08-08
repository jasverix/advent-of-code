module Y2022.Tests.Day12

open NUnit.Framework
open Y2022.Day12

let testInput =
    "Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi
"

[<Test>]
let ``Shortest path to goal`` () =
    let map = testInput |> toMap
    let distanceMap = map |> findDistanceMapDownwards (map |> getEnd)
    let shortest = distanceMap |> findShortestPathToGoal (map |> getStart)
    Assert.AreEqual(31, shortest)

[<Test>]
let ``Shortest path to bottom`` () =
    let map = testInput |> toMap
    let distanceMap = map |> findDistanceMapDownwards (map |> getEnd)
    let shortest = distanceMap |> findShortestPathToBottom map
    Assert.AreEqual(29, shortest)
