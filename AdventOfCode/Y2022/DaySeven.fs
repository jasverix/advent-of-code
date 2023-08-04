module Y2022.DaySeven

open Utils

type File = { Name: string; Size: int }

type Folder =
    { Name: string
      Folders: Map<string, Folder>
      Files: Map<string, File> }

let createFolder name =
    { Name = name
      Folders = Map.empty
      Files = Map.empty }

let createFile name size = { Name = name; Size = size }

let _addFolderTo (folders: Map<string, Folder>) folder = folders.Add(folder.Name, folder)

let _addSomeFileTo (files: Map<string, File>) (file: File option) =
    if file.IsSome then
        files.Add(file.Value.Name, file.Value)
    else
        files

let rec _addFolderOn file root (name: string, path: array<string>) =
    { Name = root.Name
      Files = root.Files
      Folders = _addFolderTo root.Folders (createFolder name |> _addFolder path file) }

and _addFolder (path: array<string>) (file: File option) root =
    if path |> Array.isEmpty then
        if file.IsSome then
            { Name = root.Name
              Files = file |> _addSomeFileTo root.Files
              Folders = root.Folders }
        else
            root
    else
        path |> unshiftItem |> _addFolderOn file root

let addFolder path root =
    if path = "/" then
        root
    else
        root |> _addFolder (path.TrimStart('/') |> split "/") None

let _addFile (fileName: string, path: array<string>) size root =
    root |> _addFolder path (Some(createFile fileName size))

let addFile path size root =
    _addFile (path |> split "/" |> popItem) size root

type Terminal = { Root: Folder; Path: string }

let exceptLast arr = arr |> Array.take (arr.Length - 1)

let fullPath path terminal =
    if path = ".." then
        terminal.Path.TrimEnd('/')
        |> split "/"
        |> exceptLast
        |> String.concat "/"
        |> fun p -> p + "/"
    elif path = "/" then
        "/"
    else
        (terminal.Path + path).TrimEnd('/') + "/"

let cd path terminal =
    let newPath = terminal |> fullPath path
    let root = terminal.Root |> addFolder newPath
    { Root = root; Path = newPath }

let indexFolder path terminal =
    let newPath = terminal |> fullPath path
    let root = terminal.Root |> addFolder newPath
    { Root = root; Path = terminal.Path }

let indexFile name size terminal =
    let filePath = (terminal |> fullPath name).TrimEnd('/')
    let root = terminal.Root |> addFile filePath size
    { Root = root; Path = terminal.Path }

let handleCommand command terminal =
    match command with
    | Regex "\$ cd (.+)" [ path ] -> terminal |> cd path
    | Regex "dir (.+)" [ path ] -> terminal |> indexFolder path
    | Regex "(\d+)\s+(.+)" [ Int size; name ] -> terminal |> indexFile name size
    | _ -> terminal
    
let handleConsole console terminal =
    console |> trim |> split "\n"
    |> Seq.fold (fun terminal command -> terminal |> handleCommand command) terminal
    
let getChildFolder name folder = folder.Folders[name]

let rec getFolderSize folder =
    let fileSize = folder.Files |> Map.values |> Seq.map(fun f -> f.Size) |> Seq.sum
    let folderSize = folder.Folders |> Map.values |> Seq.map(fun f -> f |> getFolderSize) |> Seq.sum
    fileSize + folderSize
    
let rec getAllFolders root: seq<Folder> =
    root.Folders |> Map.values
    |> Seq.map getAllFolders
    |> Seq.concat
    |> Seq.append [ root ]
    
let sumFoldersLessThan amount folders =
    folders |> Seq.map getFolderSize |> Seq.filter(fun size -> size < amount) |> Seq.sum

let initiateTerminal () = { Root = createFolder "/"; Path = "/" }

let main() =
    let input = readInputFile "07"
    let terminal = initiateTerminal() |> handleConsole input
    
    terminal.Root |> getAllFolders |> sumFoldersLessThan 10000 |> printfn "%d"
