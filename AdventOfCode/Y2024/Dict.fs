module Y2024.Dict

open System.Collections.Generic

let valueOrDefault key defaultValue (dict: IDictionary<'TKey, 'TValue>) =
    match dict.TryGetValue(key) with
    | true, value -> value
    | _ -> defaultValue