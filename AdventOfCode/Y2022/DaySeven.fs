module Y2022.DaySeven

open Utils

type File = { Name: string; Size: int }

type Folder =
    { Name: string
      Files: Map<string, File>
      Folders: Map<string, Folder> }

let createFolder name =
    { Name = name
      Files = Map.empty
      Folders = Map.empty }

let _addFolder (folders: Map<string, Folder>) (folder: Folder) = folders.Add(folder.Name, folder)
let _addFile (files: Map<string, File>) (file: File) = files.Add(file.Name, file)

let rec private _addFolderOnNameWithPath folder file (name: string, path: array<string>) =
    let child =
        if folder.Folders |> Map.containsKey name then
            folder.Folders[name]
        else
            createFolder name

    { Name = folder.Name
      Folders = _addFolder folder.Folders (_addFolderForPath path file child)
      Files = folder.Files }

and private _addFolderForPath (path: array<string>) (file: File option) folder =
    if path |> Array.isEmpty then
        if file.IsSome then
            { Name = folder.Name
              Folders = folder.Folders
              Files = _addFile folder.Files file.Value }
        else
            folder
    else
        path |> unshift |> _addFolderOnNameWithPath folder file

let addFolderForPath path folder =
    folder |> _addFolderForPath (path |> split "/") None

let addFileOnPath path file folder =
    folder |> _addFolderForPath (path |> split "/") (Some file)

let createFile name size = { Name = name; Size = size }

let rec private _getFolderByPath path folder =
    if path |> Array.isEmpty then
        Some folder
    else
        path |> unshift |> _getFolderOnNameByPath folder

and private _getFolderOnNameByPath folder (name: string, path: array<string>) =
    if folder.Folders |> Map.containsKey name then
        folder.Folders[name] |> _getFolderByPath path
    else
        None

let getChildFolderByPath (path: string) folder =
    _getFolderByPath (path |> split "/") folder

type Terminal = { Root: Folder; CD: string }

let initiateTerminal () = { Root = createFolder "/"; CD = "/" }

let rec _goToPath (path: string) terminal =
    if path = "/" then
        { Root = terminal.Root; CD = "/" }
    else
        { Root = terminal.Root |> addFolderForPath (path.TrimStart('/'))
          CD = (path.TrimEnd('/') + "/") }

let private _withoutLast array = array |> Array.take (array.Length - 1)

let cd (path: string) terminal =
    if path.StartsWith("/") then
        terminal |> _goToPath path
    elif path = ".." then
        terminal
        |> _goToPath (terminal.CD.TrimEnd('/') |> split "/" |> _withoutLast |> String.concat "/")
    else
        terminal |> _goToPath (terminal.CD + path)

let addDirectory (path: string) terminal =
    { Root = terminal.Root |> addFolderForPath ((terminal.CD + path).TrimStart('/'))
      CD = terminal.CD }

let addFile (name: string) size terminal =
    { Root = terminal.Root |> addFileOnPath terminal.CD (createFile name size)
      CD = terminal.CD }

let handleInput input terminal =
    match input with
    | Regex "\$ cd (.+)" [ path ] -> terminal |> cd path
    | Regex "\$ ls" [] -> terminal
    | Regex "dir (\w+)" [ path ] -> terminal |> addDirectory path
    | Regex "(\d+) ([\w.]+)" [ Int size; name ] -> terminal |> addFile name size
    | _ -> terminal

let handleConsole console terminal =
    console
    |> trim
    |> split "\n"
    |> Seq.fold (fun terminal command -> terminal |> handleInput command) terminal

let rec getFolderSize folder =
    let fileSize = folder.Files |> Map.values |> Seq.sumBy (fun f -> f.Size)

    let folderSize =
        folder.Folders |> Map.values |> Seq.sumBy (fun f -> f |> getFolderSize)

    fileSize + folderSize

let rec getAllFolders root : seq<Folder> =
    let folders = root.Folders
                    |> Map.values
                    |> Seq.map getAllFolders
                    |> Seq.concat
                    |> Seq.append [ root ]
    Set<Folder>(folders)

let sumFoldersBiggerThan size folders =
    folders |> Seq.map getFolderSize |> Seq.filter (fun i -> i >= size) |> Seq.sum

let main() =
    let input = readInputFile "07"
    let terminal = initiateTerminal() |> handleConsole input
    
    terminal.Root |> getAllFolders |> sumFoldersBiggerThan 10000 |> printfn "%d"