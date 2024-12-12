module Y2024.Tests.Day06

open NUnit.Framework
open Y2024.Day06

let testInput =
    "....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#..."

[<Test>]
let ``parseMapItem should parse the map item correctly`` () =
    let result = parseMapItem '^'
    Assert.That(result, Is.EqualTo GuardUp)

[<Test>]
let ``Find guard`` () =
    Assert.That(testInput |> parseMap |> findGuardPosition |> fst, Is.EqualTo(4, 6))

[<Test>]
let ``Correct map`` () =
    let map = testInput |> parseMap

    Assert.That(map |> drawMap, Is.EqualTo(testInput))
    Assert.That(map |> peek (2, 5) Up |> Option.get, Is.EqualTo Empty)

[<Test>]
let ``Correct guard path`` () =
    Assert.That(
        testInput |> parseMap |> guardPath |> Y2024.List.toString,
        Is.EqualTo(
            [ (4, 6), Up
              (4, 5), Up
              (4, 4), Up
              (4, 3), Up
              (4, 2), Up
              (4, 1), Up
              (5, 1), Right
              (6, 1), Right
              (7, 1), Right
              (8, 1), Right
              (8, 2), Down
              (8, 3), Down
              (8, 4), Down
              (8, 5), Down
              (8, 6), Down
              (7, 6), Left
              (6, 6), Left
              (5, 6), Left
              (4, 6), Left
              (3, 6), Left
              (2, 6), Left
              (2, 5), Up
              (2, 4), Up
              (3, 4), Right
              (4, 4), Right
              (5, 4), Right
              (6, 4), Right
              (6, 5), Down
              (6, 6), Down
              (6, 7), Down
              (6, 8), Down
              (5, 8), Left
              (4, 8), Left
              (3, 8), Left
              (2, 8), Left
              (1, 8), Left
              (1, 7), Up
              (2, 7), Right
              (3, 7), Right
              (4, 7), Right
              (5, 7), Right
              (6, 7), Right
              (7, 7), Right
              (7, 8), Down
              (7, 9), Down ]
            |> Y2024.List.toString
        ),
        "Could not find correct guard path\n" + (testInput |> parseMap |> drawMap)
    )

    Assert.That(testInput |> parseMap |> guardPath |> List.distinctBy fst |> List.length, Is.EqualTo 41)

[<Test>]
let ``Get loop positions`` () =
    Assert.That(
        testInput |> parseMap |> findLoopPositions |> Y2024.List.toString,
        Is.EqualTo([ 3, 6; 6, 7; 7, 7; 1, 8; 3, 8; 7, 9 ] |> List.sortBy fst |> Y2024.List.toString)
    )
