module Y2022.Tests.Day01

open NUnit.Framework
open Y2022.Day01

[<Test>]
let ``Convert some numbers to an elf`` () =
    Assert.AreEqual(
        6000,
        "1000
2000
3000"
        |> toElf
    )

let testInput = "1000
2000
3000

4000

5000
6000

7000
8000
9000

10000
"

[<Test>]
let ``Find most carry elf`` () =
    Assert.AreEqual(
        24000,
        testInput
        |> getMostCarry 1
    )

[<Test>]
let ``Find 3 most carry elf`` () =
    Assert.AreEqual(
        45000,
        testInput
        |> getMostCarry 3
    )