module Y2024.Day04

type WordMap = Map<int, Map<int, char>>

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

let getWordsOfPosition x y length (map: WordMap) =
    [ getWordLTR x y length map
      getWordRTL x y length map
      getWordTTB x y length map
      getWordBTT x y length map
      getWordBLTTR x y length map
      getWordBRTTL x y length map
      getWordTLTBR x y length map
      getWordTRTBL x y length map ]
    |> List.choose id

let getWordsWithCoordinates (word: string) map =
    map
    |> findAllCoordsOfChar (word[0])
    |> Seq.map (fun (x, y) -> getWordsOfPosition x y word.Length map |> List.map (fun w -> x, y, w))
    |> Seq.collect id

let countOccurencesOfWord (word: string) (input: string) =
    input
    |> parseInput
    |> getWordsWithCoordinates word
    |> Seq.filter (fun (_, _, w) -> w = word)
    |> Seq.length


let main () =
    Utils.readInputFile "04"
    |> countOccurencesOfWord "XMAS"
    |> printfn "Count XMAS: %A"
