open Spectre.Console
open System.IO

type Config = {
    dateFormat : string
    timeFormat : string
    dateColor : Color
    timeColor : Color
}

let parse text : Map<string, string> =
    let rec innerParse = function
        | [] -> Map.empty
        | "" :: rest -> innerParse rest
        | line :: rest ->
            let key = line[..line.IndexOf '=']
            let value = line[line.IndexOf '=' + 1 ..]
            innerParse rest
            |> Map.add key value
    text.Split('\n')
    |> List.ofArray
    |> innerParse

let configPath = "~/.config/neotimenix.config"

let defaultDateFormat = "yyyy-MM-dd"
let defaultTimeFormat = "HH:mm:ss"
let defaultDateColor = Color.White
let defaultTimeColor = Color.Green

let getConfig () =
    if File.Exists configPath |> not then
        { dateFormat = defaultDateFormat
          timeFormat = defaultTimeFormat
          dateColor  = defaultDateColor
          timeColor  = defaultTimeColor }
    else
        let text = File.ReadAllText configPath
        let parsed = parse text
        let addDefault key value map =
            if Map.containsKey key then
                map
            else
                map
                |> Map.add key value
        parsed
        |> addDefault "dateFormat" defaultDateFormat
        |> addDefault "timeFormat" defaultTimeFormat
        |> addDefault "dateColor" defaultDateColor
        |> addDefault "timeColor" defaultTimeColor
        |> fun (dict ->
            {
                dateFormat = dict["dateFormat"] 
                timeFormat = dict["timeFormat"] 
                dateColor  = dict["dateColor"] 
                timeColor  = dict["timeColor"] 
            }
        )


