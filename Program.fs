open Spectre.Console
open System
open System.Diagnostics
open System.Threading
open System.IO

let args = Environment.GetCommandLineArgs()
if Seq.contains "--generate-config" args then
    Config.generateConfig()
    printfn $"Config generated at {Config.configPath}"
    Environment.Exit(0)


let config = Config.getConfig ()

let font =
    match config.figletPath with
    | None ->
        "NeoTimeNix.colossal.flf"
        |> System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream
        |> FigletFont.Load
    | Some path -> FigletFont.Load path

let write (color : Color) (text : string) =
    FigletText(font, text)
    |> (fun p -> FigletTextExtensions.Color(p, color))
    |> AlignableExtensions.Centered
    |> AnsiConsole.Console.Write

let rec run (): unit =
    Console.Clear ()
    if config.makeNewLine then
        write config.dateColor (DateTime.Now.ToString config.dateFormat)
        write config.timeColor (DateTime.Now.ToString config.timeFormat)
    else
        let wholeLine = DateTime.Now.ToString config.dateFormat + DateTime.Now.ToString config.timeFormat
        // TODO: make it different colors for date and time?
        write config.dateColor wholeLine
    Thread.Sleep(300)
    run ()

Console.CursorVisible <- false
run ()
