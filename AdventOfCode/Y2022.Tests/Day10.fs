module Y2022.Tests.Day10

open NUnit.Framework
open Y2022.Day10

let smallProgram = "noop
addx 3
addx -5"

[<Test>]
let ``Execute simple program``() =
    let program = smallProgram |> readProgram
    let cpu = createCPU()
    let cycles = cpu |> run program
    
    Assert.AreEqual(1, cycles |> getCycle 1 |> getXDuring)
    Assert.AreEqual(1, cycles |> getCycle 2 |> getXDuring)
    Assert.AreEqual(1, cycles |> getCycle 3 |> getXDuring)
    Assert.AreEqual(4, cycles |> getCycle 4 |> getXDuring)
    Assert.AreEqual(4, cycles  |> getCycle 5 |> getXDuring)
    Assert.AreEqual(-1, cycles |> getFinalX)

let largerProgram = "addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop
"

[<Test>]
let ``Cycle strength``() =
    let program = largerProgram |> readProgram
    let cpu = createCPU()
    let cycles = cpu |> run program
    
    Assert.AreEqual(420, cycles |> getCycleStrength 20)
    Assert.AreEqual(1140, cycles |> getCycleStrength 60)
    Assert.AreEqual(1800, cycles |> getCycleStrength 100)
    Assert.AreEqual(2940, cycles |> getCycleStrength 140)
    Assert.AreEqual(2880, cycles |> getCycleStrength 180)
    Assert.AreEqual(3960, cycles |> getCycleStrength 220)
    
    Assert.AreEqual(13140, cycles |> getSignificantSignalSum)