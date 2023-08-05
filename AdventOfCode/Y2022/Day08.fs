module Y2022.Day08

open Utils

type Coordinate = int * int
type TreeMap = Map<Coordinate, int>

type Tree =
    { Position: Coordinate
      Height: int
      Map: TreeMap }

let private getRowMap (rowIndex: int) (row: string) =
    row
    |> Array.ofSeq
    |> Array.mapi (fun columnIndex char -> ((columnIndex, rowIndex), int (char.ToString())))

let toMap input : TreeMap =
    input
    |> trim
    |> split "\n"
    |> Array.mapi getRowMap
    |> Array.concat
    |> Map.ofArray

let getTree coordinate (map: TreeMap) =
    if map |> Map.containsKey coordinate then
        Some(
            { Position = coordinate
              Height = map[coordinate]
              Map = map }
        )
    else
        None

let private _north (col, row) = (col, row - 1)
let private _south (col, row) = (col, row + 1)
let private _east (col, row) = (col + 1, row)
let private _west (col, row) = (col - 1, row)

type Direction =
    | North
    | South
    | East
    | West

let step direction (tree: Tree option) =
    if tree.IsSome then
        tree.Value.Map
        |> getTree (
            match direction with
            | North -> tree.Value.Position |> _north
            | South -> tree.Value.Position |> _south
            | West -> tree.Value.Position |> _west
            | East -> tree.Value.Position |> _east
        )
    else
        None

let walk direction tree =
    tree
    |> step direction
    |> Seq.unfold (fun t -> Some(t, t |> step direction))
    |> Seq.takeWhile Option.isSome
    |> Seq.map Option.get

let anyHigher direction tree =
    Some(tree)
    |> walk direction
    |> Seq.tryFind (fun t -> t.Height >= tree.Height)
    |> Option.isSome

let takeWhileIncluding predicate (source: seq<_>) =
    seq {
        use e = source.GetEnumerator()
        let mutable latest = Unchecked.defaultof<_>

        while e.MoveNext()
              && (latest <- e.Current
                  true) do
            yield Some latest
            if not (predicate latest) then yield None
    }

let viewDistance direction tree =
    (Some tree)
    |> walk direction
    |> takeWhileIncluding (fun t -> t.Height < tree.Height)
    |> Seq.takeWhile Option.isSome
    |> Seq.length

let allShorter direction tree = not (tree |> anyHigher direction)

let isVisible tree =
    (tree |> allShorter North)
    || (tree |> allShorter South)
    || (tree |> allShorter East)
    || (tree |> allShorter West)

let scenicScore tree =
    [ North; South; East; West ]
    |> List.map (fun dir -> tree |> viewDistance dir)
    |> List.reduce (*)

let allTrees (map: TreeMap) =
    map
    |> Seq.map (fun p ->
        { Position = p.Key
          Height = p.Value
          Map = map })

let allVisibleTrees map =
    map |> allTrees |> Seq.filter (fun t -> t |> isVisible)

let allScenicScores map = map |> allTrees |> Seq.map scenicScore

let main () =
    let map = readInputFile "08" |> toMap

    map |> allVisibleTrees |> Seq.length |> printfn "%d"
    map |> allScenicScores |> Seq.max |> printfn "%d"
