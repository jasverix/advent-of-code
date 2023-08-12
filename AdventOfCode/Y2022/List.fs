module Y2022.List

let toString list = list |> List.map string |> String.concat " "

let popItem (lst: list<'T>) : 'T * list<'T> =
    if lst |> List.isEmpty then
        (Unchecked.defaultof<'T>, List.empty)
    else
        (lst |> List.last, lst |> List.take (lst.Length - 1))