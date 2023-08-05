module Y2022.Tests.Day05

open NUnit.Framework
open Y2022.Day05

let testInput = "    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2
"

let storageInput = "    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 "

let toString stack = stack |> Seq.map(fun c -> c.ToString()) |> String.concat " "


[<Test>]
let ``Storage mapping works``() =
    let storage = storageInput |> getStorage
    Assert.AreEqual("M C D", storage[2] |> toString)

[<Test>]
let ``Moving works``() =
    let storage = storageInput |> getStorage |> moveItems 2 2 1 crateMover9000
    Assert.AreEqual("Z N D C", storage[1] |> toString)
    Assert.AreEqual("M", storage[2] |> toString)
    
[<Test>]
let ``Move command works``() =
    let storage = storageInput |> getStorage |> handleMoveCommand "move 2 from 2 to 1" crateMover9000
    Assert.AreEqual("Z N D C", storage[1] |> toString)
    Assert.AreEqual("M", storage[2] |> toString)
    
[<Test>]
let ``Get top crates``() =
    Assert.AreEqual("CMZ", testInput |> getFinalStorage crateMover9000 |> getTopCrates |> Seq.map(fun c -> c.ToString()) |> String.concat "")
    Assert.AreEqual("MCD", testInput |> getFinalStorage crateMover9001 |> getTopCrates |> Seq.map(fun c -> c.ToString()) |> String.concat "")