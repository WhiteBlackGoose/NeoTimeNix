module Config

open Spectre.Console
open System.IO

type Config = {
    dateFormat : string
    timeFormat : string
    dateColor : Color
    timeColor : Color
    figletPath : Option<string>
    makeNewLine : bool
}

let parse (text : string) : Map<string, string> =
    let rec innerParse = function
        | [] -> Map.empty
        | "" :: rest -> innerParse rest
        | line :: rest ->
            let key = line[..line.IndexOf '=' - 1]
            let value = line[line.IndexOf '=' + 1 ..]
            innerParse rest
            |> Map.add key value
    text.Split('\n')
    |> List.ofArray
    |> innerParse

let configPath = $"/home/{System.Environment.UserName}/.config/neotimenix.config"

let defaultDateFormat = "yyyy-MM-dd"
let defaultTimeFormat = "HH:mm:ss"
let defaultDateColor = "FF6666"
let defaultTimeColor = "6666FF"
let defaultFigletPath = "none"
let defaultMakeNewLine = "true"

let getConfig () =
    let parseColor text =
        let i = System.Int32.Parse(text, System.Globalization.NumberStyles.HexNumber)
        Color(i / 256 / 256 % 256 |> byte, i / 256 % 256 |> byte, i % 256 |> byte)

    let parsePath text =
        if text = "none" then None
        else text |> Some

    let parseBool text =
        match text with
        | "true" -> true
        | "false" -> false
        | _ -> raise (System.Exception ($"Unexpected bool {text}"))

    if File.Exists configPath |> not then
        { dateFormat = defaultDateFormat
          timeFormat = defaultTimeFormat
          dateColor  = defaultDateColor |> parseColor
          timeColor  = defaultTimeColor |> parseColor
          figletPath = defaultFigletPath |> parsePath
          makeNewLine = defaultMakeNewLine |> parseBool
        }
    else
        let text = File.ReadAllText configPath
        let parsed = parse text
        let addDefault key value map =
            if Map.containsKey key map then
                map
            else
                map
                |> Map.add key value
        parsed
        |> addDefault "dateFormat" defaultDateFormat
        |> addDefault "timeFormat" defaultTimeFormat
        |> addDefault "dateColor" defaultDateColor
        |> addDefault "timeColor" defaultTimeColor
        |> addDefault "figletPath" defaultFigletPath
        |> addDefault "makeNewLine" defaultMakeNewLine
        |> (fun dict ->
            {
                dateFormat = dict["dateFormat"] 
                timeFormat = dict["timeFormat"] 
                dateColor  = (dict["dateColor"] |> parseColor)
                timeColor  = (dict["timeColor"] |> parseColor)
                figletPath = (dict["figletPath"] |> parsePath)
                makeNewLine = dict["makeNewLine"] |> parseBool
            }
        )

let generateConfig () =
    [
        $"dateFormat={defaultDateFormat}"
        $"timeFormat={defaultTimeFormat}"
        $"dateColor={defaultDateColor}"
        $"timeColor={defaultTimeColor}"
        $"figletPath={defaultFigletPath}"
        $"makeNewLine={defaultMakeNewLine}"
    ]
    |> String.concat "\n"
    |> (fun text -> File.WriteAllText(configPath, text))
