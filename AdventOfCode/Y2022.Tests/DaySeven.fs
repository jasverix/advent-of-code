module Y2022.Tests.DaySeven

open NUnit.Framework
open Y2022.DaySeven

[<Test>]
let ``Unshift array`` () =
    let res = [| "one"; "two"; "tree" |] |> Y2022.Utils.unshift

    Assert.AreEqual("one", res |> fst)
    Assert.AreEqual("two tree", res |> snd |> String.concat " ")

[<Test>]
let ``Create folder structure`` () =
    let root = createFolder "/" |> addFolderForPath "etc/php"
    Assert.IsTrue(root.Folders.ContainsKey("etc"))
    Assert.IsFalse(root.Folders.ContainsKey("etc/php"))
    Assert.IsTrue((root |> getChildFolderByPath "etc/php").IsSome)
    Assert.IsTrue((root |> getChildFolderByPath "etc/yeet").IsNone)

[<Test>]
let ``Create folder structure with files`` () =
    let root =
        createFolder "/"
        |> addFolderForPath "etc/php"
        |> addFileOnPath "etc" (createFile "hosts" 123)

    Assert.IsTrue(root.Folders.ContainsKey("etc"))
    Assert.IsFalse(root.Folders.ContainsKey("etc/php"))
    Assert.IsTrue(root.Folders["etc"].Files.ContainsKey("hosts"))
    Assert.AreEqual(123, root.Folders["etc"].Files["hosts"].Size)
    
[<Test>]
let ``Navigate the terminal``() =
    let terminal = initiateTerminal()
                    |> cd "/"
                    |> cd "etc"
                    |> cd "php"
                    |> cd ".."
                    |> cd "nginx"
    Assert.AreEqual("/etc/nginx/", terminal.CD)
    Assert.IsTrue(terminal.Root.Folders.ContainsKey("etc"))
    Assert.IsTrue(terminal.Root.Folders["etc"].Folders.ContainsKey("nginx"))
    Assert.IsFalse(terminal.Root.Folders.ContainsKey("yadda"))

let testInput = "$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k
"

let ``Get folder size``() =
    let terminal = initiateTerminal() |> handleConsole testInput
    Assert.AreEqual(94853, terminal.Root |> getChildFolderByPath "a" |> Option.get |> getFolderSize)
    
let ``Get all folders``() =
    let terminal = initiateTerminal() |> handleConsole testInput
    let folders = terminal.Root |> getAllFolders
    Assert.AreEqual("a d e", folders |> Seq.map(fun f -> f.Name) |> String.concat " ")

let ``Sum biggest folder``() =
    let terminal = initiateTerminal() |> handleConsole testInput
    Assert.AreEqual(95437, terminal.Root |> getAllFolders |> sumFoldersBiggerThan 10000)
