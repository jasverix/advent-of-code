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

type Range = int * int // from - to

/// get all positions covered by sensor on the given row, returned as x ranges (xFrom, xTo)
let getSensorCoverOn rowIndex sensor : Range option =
    let x, y = sensor.Pos
    let rem = sensor.BeaconDistance - abs (rowIndex - y)
    if rem >= 0 then Some(x - rem, x + rem) else None

let rangeToList (range: Range) =
    let s, e = range
    [ s..e ]

let coveredPositionsOfRow row sensors =
    let countSensorsOnLine =
        sensors
        |> Seq.collect (fun s -> [ s.Pos |> snd; s.ClosestBeacon |> snd ])
        |> Seq.where (fun y -> y = row)
        |> Seq.distinct
        |> Seq.length

    let sizeOnRow sensor =
        sensor
        |> getSensorCoverOn row
        |> Option.map rangeToList
        |> Option.defaultValue List.empty

    sensors
    |> Seq.collect sizeOnRow
    |> Set.ofSeq
    |> Set.count
    |> fun c -> c - countSensorsOnLine

let getTuningFrequency (x, y) = int64 x * 4000000L + int64 y

let getAnyFreePosition maxCoord sensors =
    /// keep range within 0..maxCoord
    let rangeWithinLimits (range: Range) : Range =
        (max 0 (range |> fst), min maxCoord (range |> snd))

    let findGap (coveredRanges: Range seq) =
        let x1, x2 =
            coveredRanges
            |> Seq.reduce (fun (from1, to1) (from2, to2) ->
                if to1 >= from2 - 1 then
                    (from1, max to2 to1)
                else
                    (-to1 - 1), to2)

        if x1 < 0 || x2 <> maxCoord then Some -x1 else None

    [ 0..maxCoord ]
    |> Seq.map (fun rowIndex ->
        sensors
        |> Seq.choose (getSensorCoverOn rowIndex)
        |> Seq.map rangeWithinLimits
        |> Seq.sortBy fst
        |> findGap)
    |> Seq.mapi (fun i x -> x |> Option.map (fun o -> (o, i)))
    |> Seq.find Option.isSome

let main () =
    let sensors = readInputFile "15" |> toSensors
    sensors |> coveredPositionsOfRow 2000000 |> printfn "Part 1: %d"

    sensors
    |> getAnyFreePosition 4000000
    |> Option.get
    |> getTuningFrequency
    |> printfn "Part 2: %d"
