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
    let countSensorsOnLine =
        sensors
        |> Seq.collect (fun s -> [ s.Pos |> snd; s.ClosestBeacon |> snd ])
        |> Seq.where (fun y -> y = row)
        |> Seq.distinct
        |> Seq.length

    let sizeOnRow row sensor =
        let diffY = abs (row - (sensor.Pos |> snd))
        let rem = sensor.BeaconDistance - diffY

        if rem >= 0 then
            let x = sensor.Pos |> fst
            [ (x - rem) .. (x + rem) ]
        else
            []

    sensors
    |> Seq.collect (sizeOnRow row)
    |> Set.ofSeq
    |> Set.count
    |> fun c -> c - countSensorsOnLine

let getTuningFrequency (x, y) = int64 x * 4000000L + int64 y

let getCorners sensors =
    let maxSensorX =
        sensors |> List.maxBy (fun s -> s.Pos |> fst) |> (fun s -> s.Pos |> fst)

    let maxSensorY =
        sensors |> List.maxBy (fun s -> s.Pos |> snd) |> (fun s -> s.Pos |> snd)

    let minSensorX =
        sensors |> List.minBy (fun s -> s.Pos |> fst) |> (fun s -> s.Pos |> fst)

    let minSensorY =
        sensors |> List.minBy (fun s -> s.Pos |> snd) |> (fun s -> s.Pos |> snd)

    let maxBeaconX =
        sensors
        |> List.maxBy (fun s -> s.ClosestBeacon |> fst)
        |> fun s -> s.ClosestBeacon |> fst

    let maxBeaconY =
        sensors
        |> List.maxBy (fun s -> s.ClosestBeacon |> snd)
        |> fun s -> s.ClosestBeacon |> snd

    let minBeaconX =
        sensors
        |> List.minBy (fun s -> s.ClosestBeacon |> fst)
        |> fun s -> s.ClosestBeacon |> fst

    let minBeaconY =
        sensors
        |> List.minBy (fun s -> s.ClosestBeacon |> snd)
        |> fun s -> s.ClosestBeacon |> snd

    (min minSensorX minBeaconX, min minSensorY minBeaconY), (max maxSensorX maxBeaconX, max maxSensorY maxBeaconY)

let minPos (x1, y1) (x2, y2) = (min x1 x2, min y1 y2)
let maxPos (x1, y1) (x2, y2) = (max x1 x2, max y1 y2)

let positionsWithinCorners (x1, y1) (x2, y2) =
    seq {
        for x in x1 |> Day14.toRange x2 do
            for y in y1 |> Day14.toRange y2 do
                yield (x, y)
    }

let getAnyTrackingSensor sensors pos =
    sensors |> List.tryFind (fun s -> (s.Pos |> distanceTo pos) <= s.BeaconDistance)

let isFree sensors pos =
    getAnyTrackingSensor sensors pos |> Option.isNone

let getAnyFreePosition sensors positions =
    positions |> Seq.tryFind (isFree sensors)
    
let getTuningFrequencyOfFreePosition maxCoord sensors =
    let minCorner, maxCorner =
        sensors |> getCorners |> (fun (c1, c2) -> (maxPos c1 (0, 0), minPos c2 (maxCoord, maxCoord)))
        
    positionsWithinCorners minCorner maxCorner |> getAnyFreePosition sensors

let main () =
    let sensors = readInputFile "15" |> toSensors
    sensors |> coveredPositionsOfRow 2000000 |> printfn "Part 1: %d"
    sensors |> getTuningFrequencyOfFreePosition 4000000 |> Option.get |> getTuningFrequency |> printfn "Part 2: %d"