module Y2022.Day16

open Utils

type Valve =
    { Key: string
      FlowRate: int
      Targets: string list }

type Tour =
    { Path: string list
      OpenValves: string list
      PressureReleasing: int
      PressureReleased: int
      MinutesPassed: int
      IsDeadEnd: bool
      Valves: Map<string, Valve> }

let currentValve tour =
    let key = tour.Path |> List.head

    if tour.Valves |> Map.containsKey key then
        tour.Valves[key]
    else
        failwithf $"Invalid valve: {key}"

let isOpen tour valve =
    tour.OpenValves |> List.contains valve.Key

let shouldOpen tour valve =
    ((valve |> isOpen tour) || valve.FlowRate = 0) |> not

let beenHereBefore tour =
    tour.Path |> List.tail |> List.contains (tour.Path |> List.head)

let allTargetsVisited tour =
    tour
    |> currentValve
    |> fun valve -> valve.Targets
    |> List.forall (fun target -> tour.Path |> List.contains target)

let nextStep tour =
    if tour.IsDeadEnd || tour.MinutesPassed = 30 then
        [ { tour with
              PressureReleased = tour.PressureReleased + tour.PressureReleasing } ]
    elif tour |> currentValve |> shouldOpen tour then
        [ { tour with
              OpenValves = (tour.Path |> List.head) :: tour.OpenValves
              MinutesPassed = tour.MinutesPassed + 1
              PressureReleased = tour.PressureReleased + tour.PressureReleasing
              PressureReleasing = tour.PressureReleasing + (tour |> currentValve).FlowRate } ]
    elif (tour |> allTargetsVisited) && (tour |> beenHereBefore) then
        [ { tour with
              PressureReleased = tour.PressureReleased + tour.PressureReleasing
              IsDeadEnd = true } ]
    else
        tour
        |> currentValve
        |> fun valve -> valve.Targets
        |> List.map (fun target ->
            { tour with
                Path = target :: tour.Path
                PressureReleased = tour.PressureReleased + tour.PressureReleasing
                MinutesPassed = tour.MinutesPassed + 1 })

let initTour valves =
    { Path = [ "AA" ]
      OpenValves = []
      PressureReleasing = 0
      PressureReleased = 0 
      MinutesPassed = 0
      IsDeadEnd = false
      Valves = valves }

let runTours minutes valves =
    [ 0..minutes ]
    |> List.fold (fun tours _ -> tours |> List.collect nextStep) [ initTour valves ]

let toValve input =
    match input |> Str.trim with
    | Regex "Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? ([\w,\s]+)$" [ valveKey; Int flowRate; targets ] ->
        { Key = valveKey
          FlowRate = flowRate
          Targets = targets |> Str.split "," |> Array.map Str.trim |> List.ofArray }
    | _ -> failwithf $"Invalid input: {input}"

let toValves input =
    input
    |> Str.trim
    |> Str.split "\n"
    |> Array.map toValve
    |> Array.map (fun valve -> valve.Key, valve)
    |> Map.ofArray

let bestTour tours =
    tours |> List.maxBy (fun t -> t.PressureReleased)
