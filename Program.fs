open Spectre.Console
open System
open System.Diagnostics
open System.Threading
open System.IO

let args = Environment.GetCommandLineArgs ()

let dateFormat = if args.Length > 2 then args[2] else "yyyy-MM-dd"
let timeFormat = if args.Length > 3 then args[3] else "HH:mm:ss"

let font = FigletFont.Load "./colossal.flf"

let write (color : Color) (text : string) =
    FigletText(font, text)
    |> (fun p -> FigletTextExtensions.Color(p, color))
    |> AlignableExtensions.Centered
    |> AnsiConsole.Console.Write

let rec run (): unit =
    Console.Clear ()
    write Color.White (DateTime.Now.ToString(dateFormat))
    write Color.Green (DateTime.Now.ToString(timeFormat))
    Thread.Sleep(300)
    run ()

Console.CursorVisible <- false
run ()
