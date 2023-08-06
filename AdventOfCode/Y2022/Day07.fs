module Y2022.Day07

open Utils

type File = { Name: string; Size: int }

type Folder =
    { Name: string
      Folders: Map<string, Folder>
      Files: Map<string, File> }

let createFolder name =
    if name = "" then
        failwith "empty name given"
    else
        { Name = name
          Folders = Map.empty
          Files = Map.empty }

let createFile name size = { Name = name; Size = size }

let private _plainRootPath (path: string) = path.Trim().TrimStart('/').TrimEnd('/')

let _addFolderTo (folders: Map<string, Folder>) folder = folders.Add(folder.Name, folder)
let _addFileTo (files: Map<string, File>) (file: File) = files.Add(file.Name, file)

let _getOrCreateFolder name root =
    if root.Folders |> Map.containsKey name then
        root.Folders[name]
    else
        createFolder name

let rec _addFolderOn file root (name: string, path: array<string>) =
    { root with
        Folders =
            root
            |> _getOrCreateFolder name
            |> _addFolderByPath path file
            |> _addFolderTo root.Folders }

and _addFolderByPath (path: array<string>) (file: File option) root =
    if path |> Array.isEmpty then
        if file.IsSome then
            { root with
                Files = file.Value |> _addFileTo root.Files }
        else
            root
    else
        path |> unshiftItem |> _addFolderOn file root

let addFolder path root =
    if path = "/" then
        root
    else
        root |> _addFolderByPath (path |> _plainRootPath |> split "/") None

let _addFile (fileName: string, path: array<string>) size root =
    root |> _addFolderByPath path (Some(createFile fileName size))

let addFile path size root =
    _addFile (path |> _plainRootPath |> split "/" |> popItem) size root

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
    console
    |> trim
    |> split "\n"
    |> Seq.fold (fun terminal command -> terminal |> handleCommand command) terminal

let getChildFolder name folder = folder.Folders[name]

let rec getFolderSize folder =
    let fileSize = folder.Files |> Map.values |> Seq.map (fun f -> f.Size) |> Seq.sum

    let folderSize =
        folder.Folders |> Map.values |> Seq.map (fun f -> f |> getFolderSize) |> Seq.sum

    fileSize + folderSize

let rec getAllFolders root : seq<Folder> =
    root.Folders
    |> Map.values
    |> Seq.map getAllFolders
    |> Seq.concat
    |> Seq.append [ root ]

let getFolderSizes folders = folders |> Seq.map getFolderSize

let sumFoldersLessThan amount folderSizes =
    folderSizes |> Seq.filter (fun size -> size < amount) |> Seq.sum

let initiateTerminal () = { Root = createFolder "/"; Path = "/" }

let main () =
    let input = readInputFile "07"
    let terminal = initiateTerminal () |> handleConsole input
    let folderSizes = terminal.Root |> getAllFolders |> getFolderSizes |> Seq.toList

    folderSizes |> sumFoldersLessThan 100000 |> printfn "%d"

    let totalSizeAvailable = 70000000
    let free = totalSizeAvailable - (folderSizes |> List.max)
    let neededSize = 30000000

    folderSizes
    |> Seq.filter (fun size -> (free + size) >= neededSize)
    |> Seq.min
    |> printfn "%d"
