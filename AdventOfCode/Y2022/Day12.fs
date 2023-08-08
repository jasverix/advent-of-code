module Y2022.Day12

open Utils

type Elevation = char
type Position = int * int
type HeightMap = Map<Position, Elevation>
type Distance = int
type DistanceMap = Map<Position, Distance>

let parseElevation char =
    match char with
    | 'S' -> 0
    | 'E' -> int 'z' - (int 'a')
    | c -> int c - int 'a'

let getStart (map: HeightMap) = map |> Map.findKey (fun _ s -> s = 'S')
let getEnd (map: HeightMap) = map |> Map.findKey (fun _ s -> s = 'E')

let canComeFrom (target: Elevation) (source: Elevation) =
    parseElevation target >= (parseElevation source - 1)

let parseRow (row: string) = row |> Seq.toArray

let private _toRow (lat: int) (row: string) : (Position * Elevation) array =
    row |> Array.ofSeq |> Array.mapi (fun lng char -> ((lat, lng), char))

let toMap input : HeightMap =
    input |> trim |> split "\n" |> Array.mapi _toRow |> Array.concat |> Map.ofArray

let findNeighbours (map: HeightMap) (pos: Position) : Position list =
    let add (a1, a2) (b1, b2) = a1 + b1, a2 + b2

    match map |> Map.tryFind pos with
    | None -> []
    | Some height ->
        [ (0, 1); (1, 0); (0, -1); (-1, 0) ]
        |> Seq.map (add pos)
        |> Seq.choose (fun neighbour ->
            map
            |> Map.tryFind neighbour
            |> Option.filter (fun v -> height |> canComeFrom v)
            |> Option.map (fun _ -> neighbour))
        |> Seq.toList

let findDistanceMapDownwards (origin: Position) terrain : DistanceMap =
    let rec propagateDistance distances positions =
        match positions with
        | [] -> distances
        | (pos, distance) :: xs when Map.tryFind pos distances |> Option.exists (fun v -> v <= distance) ->
            propagateDistance distances xs
        | (pos, distance) :: xs ->
            let newDistanceMap = distances |> Map.add pos distance

            let newNeighbours =
                findNeighbours terrain pos |> List.map (fun p -> (p, distance + 1))

            let neighborsToPropagate =
                newNeighbours
                |> Seq.filter (fun (pos, dist) ->
                    newDistanceMap |> Map.containsKey pos |> not
                    || newDistanceMap |> Map.tryFind pos |> Option.exists (fun v -> v > dist))
                |> Seq.toList

            propagateDistance newDistanceMap (neighborsToPropagate @ xs)

    propagateDistance Map.empty [ (origin, 0) ]

let findShortestPathToGoal goal distanceMap =
    distanceMap |> Map.find goal
    
let findShortestPathToBottom map distanceMap =
    let zeroLevels = map |> Map.filter (fun k v -> v = 'a') |> Map.keys
    distanceMap |> Map.filter (fun k _ -> zeroLevels.Contains(k)) |> Map.values |> Seq.min    
    
let main() =
    let map = readInputFile "12" |> toMap
    let distanceMap = map |> findDistanceMapDownwards (map |> getEnd)
    let start = map |> getStart
    distanceMap |> findShortestPathToGoal start |> printfn "%d"
    distanceMap |> findShortestPathToBottom map |> printfn "%d"