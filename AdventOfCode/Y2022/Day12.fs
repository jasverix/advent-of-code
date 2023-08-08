module Y2022.Day12

open Utils

type Elevation = char
type Position = int * int
type HeightMap = Map<Position, Elevation>

type Spot =
    { Pos: Position
      Map: HeightMap
      Elevation: Elevation }

let getElevation char =
    match char with
    | 'S' -> 'a'
    | 'E' -> 'z'
    | c -> c

let getSpot pos map =
    if map |> Map.containsKey pos then
        Some
            { Pos = pos
              Map = map
              Elevation = getElevation map[pos] }
    else
        None

let getSpot2 map pos = getSpot pos map

let getStart (map: HeightMap) =
    map |> Map.findKey (fun _ s -> s = 'S') |> getSpot2 map |> Option.get

let getEnd (map: HeightMap) =
    map |> Map.findKey (fun _ s -> s = 'E') |> getSpot2 map |> Option.get

let canGoto (target: Elevation) (source: Elevation) = int target <= (int source + 1)

type Direction =
    | Up
    | Left
    | Right
    | Down

type Path = Position list

let step dir (lat, lng) =
    match dir with
    | Up -> (lat - 1, lng)
    | Down -> (lat + 1, lng)
    | Left -> (lat, lng - 1)
    | Right -> (lat, lng + 1)

let stepSpot dir spot =
    spot.Pos |> step dir |> getSpot2 spot.Map

let _getSurroundingSpots pos map =
    [ getSpot (pos |> step Down) map
      getSpot (pos |> step Up) map
      getSpot (pos |> step Right) map
      getSpot (pos |> step Left) map ]
    |> List.filter Option.isSome
    |> List.map Option.get

let getSurroundingSpots spot = _getSurroundingSpots spot.Pos spot.Map

let getAccessibleSpots spot =
    spot
    |> getSurroundingSpots
    |> List.filter (fun s -> spot.Elevation |> canGoto s.Elevation)

let walk dir (start: Spot) =
    start
    |> stepSpot dir
    |> Seq.unfold (fun spot ->
        if spot.IsSome then
            let nextSpot = spot.Value |> stepSpot dir

            if nextSpot.IsSome then
                if spot.Value.Elevation |> canGoto nextSpot.Value.Elevation then
                    Some(spot, nextSpot)
                else
                    None
            else
                None
        else
            None)
    |> Seq.takeWhile Option.isSome
    |> Seq.map Option.get

let nextSteps (currentPath: Path) (currentSpot: Spot) : Path list =
    currentSpot
    |> getAccessibleSpots
    |> List.filter (fun s -> not (List.contains s.Pos currentPath))
    |> List.map (fun s -> s.Pos :: currentPath)

let nextStepsOrSelf currentPath currentSpot target : Path list =
    let next = nextSteps currentPath currentSpot

    if next.Length = 0 then
        if currentPath |> List.contains target then
            [ currentPath ]
        else
            []
    else
        next

let getNextSteps map (paths: Path list) target =
    paths
    |> List.map (fun path -> nextStepsOrSelf path (getSpot path.Head map).Value target)
    |> List.concat

let getShortestPath (paths: Path list) (pos: Position) =
    let findIndex (index: int option) =
        match index with
        | Some index -> index
        | None -> -1

    let reverse index = (paths.Length - index)

    paths
    |> List.minBy (fun path -> path |> List.tryFindIndexBack (fun p -> p = pos) |> findIndex |> reverse)

let onlyShortestPaths (paths: Path list) =
    paths
    |> List.map List.head
    |> List.map (getShortestPath paths)
    |> Set.ofList
    |> Set.toList

let allPathsToTarget target spot =
    let mutable paths = getNextSteps spot.Map [ [ spot.Pos ] ] target
    let withTarget p = p |> List.contains target

    while paths.Length <> (paths |> List.filter withTarget).Length do
        paths <- getNextSteps spot.Map paths target |> onlyShortestPaths

    paths |> List.filter (fun p -> p |> List.contains target) |> List.map List.tail


let containsSpot spot (path: Path) = path |> List.contains spot.Pos

let pathsToEnd map =
    map |> getStart |> allPathsToTarget (map |> getEnd |> (fun s -> s.Pos))

let private _toRow (lat: int) (row: string) : (Position * Elevation) array =
    row |> Array.ofSeq |> Array.mapi (fun lng char -> ((lat, lng), char))

let toMap input : HeightMap =
    input |> trim |> split "\n" |> Array.mapi _toRow |> Array.concat |> Map.ofArray

let main () =
    let map = readInputFile "12" |> toMap
    map |> pathsToEnd |> List.map List.length |> List.min |> printfn "%d"
