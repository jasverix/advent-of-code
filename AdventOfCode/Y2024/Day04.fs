module Y2024.Day04

type Direction =
    | LTR
    | RTL
    | TTB
    | BTT
    | BLTTR
    | BRTTL
    | TLTBR
    | TRTBL

type WordMap = Map<int, Map<int, char>>
type WordWithDirection = string * Direction
type WordWithCoordinates = int * int * WordWithDirection

let toMapLine (line: string) : Map<int, char> =
    line.ToCharArray() |> Array.mapi (fun x c -> (x, c)) |> Map.ofArray

let parseInput (input: string) : WordMap =
    input.Split('\n')
    |> Array.map toMapLine
    |> Array.mapi (fun y line -> (y, line))
    |> Map.ofArray

let validateWord length (word: string) =
    if word.Length = length then Some word else None

let getWord x y length predicate =
    try
        [ 0 .. length - 1 ] |> List.map predicate |> Str.ofSeq |> validateWord length
    with _ ->
        None

let getWordLTR x y length (map: WordMap) =
    getWord x y length (fun i -> map[y][x + i])

let getWordRTL x y length (map: WordMap) =
    getWord x y length (fun i -> map[y][x - i])

let getWordTTB x y length (map: WordMap) =
    getWord x y length (fun i -> map[y + i][x])

let getWordBTT x y length (map: WordMap) =
    getWord x y length (fun i -> map[y - i][x])

let getWordBLTTR x y length (map: WordMap) =
    getWord x y length (fun i -> map[y - i][x + i])

let getWordBRTTL x y length (map: WordMap) =
    getWord x y length (fun i -> map[y - i][x - i])

let getWordTLTBR x y length (map: WordMap) =
    getWord x y length (fun i -> map[y + i][x + i])

let getWordTRTBL x y length (map: WordMap) =
    getWord x y length (fun i -> map[y + i][x - i])

let findAllCoordsOfChar (char: char) (map: WordMap) =
    map
    |> Map.toSeq
    |> Seq.collect (fun (y, line) -> line |> Map.toSeq |> Seq.map (fun (x, c) -> (x, y, c)))
    |> Seq.filter (fun (_, _, c) -> c = char)
    |> Seq.map (fun (x, y, _) -> (x, y))

let getWordsOfPosition x y length (map: WordMap) : WordWithDirection list =
    [ getWordLTR x y length map, LTR
      getWordRTL x y length map, RTL
      getWordTTB x y length map, TTB
      getWordBTT x y length map, BTT
      getWordBLTTR x y length map, BLTTR
      getWordBRTTL x y length map, BRTTL
      getWordTLTBR x y length map, TLTBR
      getWordTRTBL x y length map, TRTBL ]
    |> List.filter (fun (word, _) -> word.IsSome)
    |> List.map (fun (word, dir) -> word.Value, dir)

let getWordsWithCoordinates (word: string) map : WordWithCoordinates seq =
    map
    |> findAllCoordsOfChar (word[0])
    |> Seq.map (fun (x, y) -> getWordsOfPosition x y word.Length map |> List.map (fun w -> x, y, w))
    |> Seq.collect id
    |> Seq.filter (fun (_, _, w) -> fst w = word)

let countOccurencesOfWord (word: string) (input: string) =
    input |> parseInput |> getWordsWithCoordinates word |> Seq.length

let indexWordsWithCoordinatesInMap wordsWithCoordinates =
    wordsWithCoordinates
    |> Seq.fold
        (fun acc (x, y, word) ->
            let innerMap =
                match Map.tryFind x acc with
                | Some map -> map
                | None -> Map.empty

            let updatedInnerMap = Map.add y word innerMap
            Map.add x updatedInnerMap acc)
        Map.empty

let getWordFromMap x y (map: Map<int, Map<int, WordWithDirection>>) : WordWithCoordinates = x, y, map[x][y]

let wordsAreCrossing (w1: WordWithCoordinates) (w2: WordWithCoordinates) =
    if w1 = w2 then
        false
    else
        let x1, y1, (word1, dir1) = w1
        let x2, y2, (word2, dir2) = w2

        if dir1 = TLTBR then
            if dir2 = TRTBL then
                y1 = y2 && abs (x2-x1) = 2
            else if dir2 = BLTTR then
                abs (x2-x1) = 1 && abs (y2-y1) = 2
            else if dir2 = BRTTL then
                x1 = x2 && abs (y2-y1) = 2
            else
                false
        else if dir1 = TRTBL then
            if dir2 = TLTBR then
                y1 = y2 && abs (x2-x1) = 2
            else if dir2 = BLTTR then
                x1 = x2 && abs (y2-y1) = 2
            else if dir2 = BRTTL then
                abs (x2-x1) = 1 && abs (y2-y1) = 2
            else
                false
        else if dir1 = BLTTR then
            if dir2 = TLTBR then
                abs (x2-x1) = 1 && abs (y2-y1) = 2
            else if dir2 = TRTBL then
                x1 = x2 && abs (y2-y1) = 2
            else if dir2 = BRTTL then
                y1 = y2 && abs (x2-x1) = 2
            else
                false
        else if dir1 = BRTTL then
            if dir2 = TLTBR then
                x1 = x2 && abs (y2-y1) = 2
            else if dir2 = TRTBL then
                abs (x2-x1) = 1 && abs (y2-y1) = 2
            else if dir2 = BLTTR then
                y1 = y2 && abs (x2-x1) = 2
            else false
        else false

let hasAnyCrossingWords (list: WordWithCoordinates list) (word: WordWithCoordinates) =
    list |> List.exists (fun w -> wordsAreCrossing w word)

let getAllCrossingWords list =
    list |> List.filter (hasAnyCrossingWords list)

let findWordsWithCoordinates word input =
    input
    |> parseInput
    |> getWordsWithCoordinates word
    |> List.ofSeq

let findCrossMases input =
    input
    |> findWordsWithCoordinates "MAS"
    |> getAllCrossingWords

let main () =
    Utils.readInputFile "04"
    |> countOccurencesOfWord "XMAS"
    |> printfn "Count XMAS: %A"
