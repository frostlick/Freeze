module FreezeClient.Models.Program

open System
open Serilog
open Serilog.Extensions.Logging
open Elmish
open Elmish.WPF

let main window =
    let logger =
        LoggerConfiguration()
            .MinimumLevel.Override("Elmish.WPF.Update", Events.LogEventLevel.Verbose)
            .MinimumLevel.Override("Elmish.WPF.Bindings", Events.LogEventLevel.Verbose)
            .MinimumLevel.Override("Elmish.WPF.Performance", Events.LogEventLevel.Verbose)
            .WriteTo.Console()
            .CreateLogger()
    WpfProgram.mkProgramWithCmdMsg MainWindow.init MainWindow.update
        MainWindow.Platform.bindings MainWindow.Platform.toCmd
    |> WpfProgram.withSubscription (fun _ -> Cmd.ofSub MainWindow.Platform.timerTick)
    |> WpfProgram.withLogger (new SerilogLoggerFactory(logger))
    |> WpfProgram.startElmishLoop window
