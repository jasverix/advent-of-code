module Y2024.Tests.Day03

open NUnit.Framework
open Y2024.Day03

let testInput = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))"

[<Test>]
let ``Handle mul requests`` () =
    Assert.That("mul(2,42)" |> getMultiplications, Is.EqualTo 84)
    Assert.That("mul(2,4)mul(3,3)" |> getMultiplications, Is.EqualTo 17)
    
[<Test>]
let ``Handle test input part 1`` () =
    Assert.That(testInput |> getMultiplications, Is.EqualTo 48)