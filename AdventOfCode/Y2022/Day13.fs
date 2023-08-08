module Y2022.Day13

type PacketContent =
    | Single of int
    | Sublist of PacketContent list

type Packet = PacketContent list
type PacketPair = Packet * Packet

type Comparison =
    | Equal
    | Greater
    | Lesser

let rec compareItemTo item2 item1 =
    match item2 with
    | Single i2 ->
        match item1 with
        | Single i1 ->
            if i1 = i2 then Equal
            elif i1 > i2 then Greater
            else Lesser
        | Sublist _ -> item1 |> compareItemTo (Sublist [ Single i2 ])
    | Sublist l2 ->
        match item1 with
        | Single i1 -> (Sublist [ Single i1 ]) |> compareItemTo item2
        | Sublist l1 -> l1 |> _listIsGreaterThan l2

and private _listIsGreaterThan (list2: PacketContent list) (list1: PacketContent list) =
    list1
    |> List.mapi (fun i item -> (i, item))
    |> List.choose (fun (i, item1) ->
        if i > (list2.Length - 1) then
            Some Greater
        else
            let item2 = list2[i]
            let c = item1 |> compareItemTo item2

            match c with
            | Equal -> None
            | Greater -> Some Greater
            | Lesser -> Some Lesser)
    |> List.tryHead
    |> Option.defaultWith (fun () -> if list2.Length > list1.Length then Lesser else Equal)

let toPacket (input: string) : Packet =
    let rec parse (acc: PacketContent list) rest =
        match rest with
        | [] -> acc, []
        | '[' :: tail ->
            let innerlist, remaining = parse [] tail
            parse (Sublist(List.rev innerlist) :: acc) remaining
        | ']' :: tail -> acc, tail
        | ',' :: tail -> parse acc tail
        | _ ->
            let number =
                rest |> List.takeWhile System.Char.IsDigit |> List.ofSeq |> System.String.Concat

            let remaining = rest |> List.skipWhile System.Char.IsDigit

            parse (Single(int number) :: acc) remaining

    let parsedList, _ = parse [] (input |> Utils.trim |> List.ofSeq)
    parsedList

let toPacketPair input : PacketPair =
    input
    |> Utils.trim
    |> Utils.splitAt "\n"
    |> fun (a, b) -> (a |> toPacket, b |> toPacket)

let comparePackets p2 p1 =
    Sublist(p1) |> compareItemTo (Sublist(p2))

let packetPairIsCorrect (item1: Packet, item2: Packet) =
    Sublist(item2) |> compareItemTo (Sublist(item1)) |> (fun c -> c = Greater)

let getPairs input =
    input |> Utils.trim |> Utils.split "\n\n" |> Seq.map toPacketPair |> List.ofSeq

let getIndexesOfRightPairs (pairs: PacketPair list) =
    pairs
    |> List.mapi (fun i pair -> (i, pair))
    |> List.filter (fun (_, pair) -> packetPairIsCorrect pair)
    |> List.map (fun (i, _) -> (i + 1))
    |> List.sum

let unbundlePairs (pairs: PacketPair list) : Packet list =
    pairs |> Seq.map (fun (p1, p2) -> [ p1; p2 ]) |> Seq.concat |> List.ofSeq
    
let orderPackets (packets: Packet list) =
    packets
    |> List.sortWith (fun p1 p2 ->
        match (p1 |> comparePackets p2) with
        | Greater -> 1
        | Lesser -> -1
        | Equal -> 0)
    
let getIndexOfPacket packet packets =
    packets
    |> List.tryFindIndex(fun p -> p = packet)
    |> fun index -> match index with
                    | Some i -> (i + 1)
                    | None -> -1

let main () =
    let pairs = Utils.readInputFile "13" |> getPairs
    pairs |> getIndexesOfRightPairs |> printfn "%d"
    
    let packets = pairs |> unbundlePairs
    let dividerOne = "[[2]]" |> toPacket
    let dividerTwo = "[[6]]" |> toPacket
    let sortedPackets = dividerTwo::dividerOne::packets |> orderPackets
    
    let indexOne = sortedPackets |> getIndexOfPacket dividerOne
    let indexTwo = sortedPackets |> getIndexOfPacket dividerTwo
    
    (indexOne * indexTwo) |> printfn "%d"