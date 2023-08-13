module Y2022.Tests.Day15

open NUnit.Framework
open Y2022.Day15

[<Test>]
let ``Get range from position`` () =
    let positions = (10, 10) |> positionsInRange 5 |> Set.ofSeq

    Assert.AreEqual(
        "(5, 10) (6, 9) (6, 10) (6, 11) (7, 8) (7, 9) (7, 10) (7, 11) (7, 12) (8, 7) (8, 8) (8, 9) (8, 10) (8, 11) (8, 12) (8, 13) (9, 6) (9, 7) (9, 8) (9, 9) (9, 10) (9, 11) (9, 12) (9, 13) (9, 14) (10, 5) (10, 6) (10, 7) (10, 8) (10, 9) (10, 10) (10, 11) (10, 12) (10, 13) (10, 14) (10, 15) (11, 6) (11, 7) (11, 8) (11, 9) (11, 10) (11, 11) (11, 12) (11, 13) (11, 14) (12, 7) (12, 8) (12, 9) (12, 10) (12, 11) (12, 12) (12, 13) (13, 8) (13, 9) (13, 10) (13, 11) (13, 12) (14, 9) (14, 10) (14, 11) (15, 10)",
        positions |> Set.toList |> Y2022.List.toString
    )

[<Test>]
let ``Get distance between`` () =
    Assert.AreEqual(5, (10, 10) |> distanceTo (15, 10))
    Assert.AreEqual(5, (10, 10) |> distanceTo (7, 8))
    Assert.AreEqual(5, (10, 10) |> distanceTo (6, 9))
    Assert.AreEqual(5, (10, 10) |> distanceTo (5, 10))
    Assert.AreEqual(5, (10, 10) |> distanceTo (11, 14))
    Assert.AreEqual(5, (10, 10) |> distanceTo (11, 6))
    Assert.AreEqual(5, (10, 10) |> distanceTo (10, 15))

let testInput =
    "Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3
"

[<Test>]
let ``Covered positions at row 10`` () =
    Assert.AreEqual(26, testInput |> toSensors |> coveredPositionsOfRow 10)

[<Test>]
let ``Tuning frequency of 14,11`` () =
    Assert.AreEqual(56000011, (14, 11) |> getTuningFrequency)

[<Test>]
let ``Get corners`` () =
    Assert.AreEqual("((-2, 0), (25, 22))", testInput |> toSensors |> getCorners |> string)

[<Test>]
let ``Get tracking sensors`` () =
    let sensors = testInput |> toSensors
    Assert.AreEqual("(8, 7)", getAnyTrackingSensor sensors (8, -2) |> Option.get |> fun s -> s.Pos |> string)

[<Test>]
let ``Get free positions`` () =
    let sensors = testInput |> toSensors
    let position =
        sensors |> getTuningFrequencyOfFreePosition 20

    Assert.AreEqual("Some((14, 11))", position |> string)
