module Y2022.Day15

open Utils

type Coordinate = int * int

type Sensor =
    { Pos: Coordinate
      ClosestBeacon: Coordinate
      BeaconDistance: int }

let positionsInRange (range: int) (pos: Coordinate) : Coordinate seq =
    let x, y = pos

    seq {
        for i in -range .. range do
            for j in -range .. range do
                if abs i + abs j <= range then
                    yield (x + i, y + j)
    }

let distanceTo (x2, y2) (x1, y1) = abs (x1 - x2) + abs (y1 - y2)

let beaconRange sensor = sensor.BeaconDistance

let positionsInBeaconRange sensor =
    sensor |> beaconRange |> (fun range -> sensor.Pos |> positionsInRange range)

let coveredPositions sensors =
    seq {
        for s in sensors do
            for p in s |> positionsInBeaconRange do
                yield p
    }
    |> Set.ofSeq

let beaconPositions sensors =
    sensors |> Seq.map (fun s -> s.ClosestBeacon) |> Set.ofSeq

let toSensor input =
    match input |> Str.trim with
    | Regex "Sensor at x=([-\d]+), y=([-\d]+): closest beacon is at x=([-\d]+), y=([-\d]+)" [ Int x
                                                                                              Int y
                                                                                              Int bx
                                                                                              Int by ] ->
        let pos = x, y
        let beacon = bx, by

        { Pos = pos
          ClosestBeacon = beacon
          BeaconDistance = pos |> distanceTo beacon }
    | _ -> failwith "Invalid input"

let toSensors input =
    input |> Str.trim |> Str.split "\n" |> Array.map toSensor |> List.ofArray

let noBeacons beacons coordinates =
    coordinates |> Set.filter (fun c -> not (Set.contains c beacons))

let coveredPositionsOfRow row sensors =
    let beacons = sensors |> beaconPositions

    sensors
    |> coveredPositions
    |> Set.filter (fun (x, _) -> x = row)
    |> noBeacons beacons
    |> Set.count

let main () =
    readInputFile "15"
    |> toSensors
    |> coveredPositionsOfRow 2000000
    |> printfn "Part 1: %d"
