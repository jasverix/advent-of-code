module Y2022.Tests.Day09

open NUnit.Framework
open Y2022.Day09

[<Test>]
let ``Touching ints``() =
    Assert.IsTrue(4 |> isCloseTo 5)
 
[<Test>]
let ``Edge closer``() =
    Assert.AreEqual((2,4), (2,3) |> edgeCloser (2,5))
    Assert.AreEqual((3,4), (2,4) |> edgeCloser (5,4))
    Assert.AreEqual((3,5), (2,4) |> edgeCloser (5,6))
    Assert.AreEqual((2,4), (2,4) |> edgeCloser (2,4))
    Assert.AreEqual((2,4), (2,4) |> edgeCloser (3,4))
    Assert.AreEqual((2,4), (2,4) |> edgeCloser (2,5))
    Assert.AreEqual((2,4), (2,4) |> edgeCloser (3,5))
    
let testInput = "R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2
"

[<Test>]
let ``Tracking of tail``() =
    let snake = createSnake 2
    let moves = snake |> runCommands testInput
    let tailPositions = moves |> getTailPositions
    Assert.AreEqual(13, tailPositions |> Set.count)
    
let biggerTestInput = "R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20
"

[<Test>]
let ``Tracking of long tail``() =
    let snake = createSnake 10
    let moves = snake |> runCommands biggerTestInput
    let tailPositions = moves |> getTailPositions
    Assert.AreEqual(36, tailPositions |> Set.count)