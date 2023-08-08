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
let ``Can go to`` () = Assert.IsTrue('d' |> canGoto 'e')

[<Test>]
let ``Next step from (4,2)`` () =
    let map = testInput |> toMap
    let spot = map |> getSpot (4, 2) |> Option.get

    Assert.AreEqual('d', spot.Elevation)

    let nextSteps = spot |> nextSteps []

    Assert.AreEqual(
        [ [ (3, 2) ];  [ (4, 3) ]; [ (4, 1) ] ] |> List.map string |> String.concat " ",
        nextSteps |> List.map string |> String.concat " "
    )
    
[<Test>]
let ``Next step from (0,6)`` () =
    let map = testInput |> toMap
    let spot = map |> getSpot (0, 6) |> Option.get

    Assert.AreEqual('n', spot.Elevation)

    let nextSteps = spot |> nextSteps []

    Assert.AreEqual(
        [ [ (0, 7) ];  [ (0, 5) ] ] |> List.map string |> String.concat " ",
        nextSteps |> List.map string |> String.concat " "
    )

[<Test>]
let ``Next step of viable path``() =
    let toString list = list |> List.map string |> String.concat " "
    let viablePath = [ (0,0); (1,0); (1,1); (2,1); (2,2); (3,2); (4,2); (4,3); (4,4); (4,5); (4,6) ] |> List.rev
    let map = testInput |> toMap
    let nextSteps = nextStepsOrSelf viablePath (map |> getSpot (4,6) |> Option.get) (map |> getEnd).Pos
    Assert.AreEqual([ (4,7) :: viablePath ] |> toString, nextSteps |> toString)

[<Test>]
let ``Shortest path to goal`` () =
    let map = testInput |> toMap
    let paths = pathsToEnd map
    let shortest = paths |> List.map List.length |> List.min

    Assert.AreEqual(31, shortest)
