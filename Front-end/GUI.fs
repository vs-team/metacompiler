module GUI

open System   
open System.Windows
open System.Windows.Controls
open System.Windows.Media
open System.Windows.Shapes
open System.Runtime.InteropServices

[<DllImport("kernel32.dll")>]
extern IntPtr GetConsoleWindow()
[<DllImport("user32.dll")>]
extern bool ShowWindow(IntPtr hWnd, int nCmdShow)

type HelpWindow(samples : List<string * string>) =
  inherit Window()

  do base.Width <- 600.0

  let stackPanel = new StackPanel()

  let helpText = new TextBox()
  do helpText.FontFamily <- FontFamily("consolas")
  do helpText.IsReadOnly <- true
  do helpText.TextWrapping <- TextWrapping.WrapWithOverflow
  do helpText.Text <- @"Use the drop-down menu to select a set of transformation rules to run. Write a sample program and see what the rules return for it.
  
Possible inputs are:
 " + 
     ([for path, sample in samples -> "- for " + path + "\t\t\t" + sample ] |> Seq.reduce (fun a b -> a + "\n" + b)) +
       @" 

Just copy-and-paste these samples and check the corresponding rules to get a feel for the system operation."
  do helpText.Padding <- Thickness(10.0)
  do helpText.Margin <- Thickness(10.0)

  do stackPanel.Children.Add helpText |> ignore

  do base.Content <- stackPanel



type RuleEditorWindow(path, file) =
  inherit Window()

  let path = System.IO.Path.Combine(path, file)

  do base.WindowState <- WindowState.Maximized

  let editorText = new TextBox()
  do editorText.FontFamily <- FontFamily("consolas")
  do editorText.Text <- System.IO.File.ReadAllText(path)
  let update_text _ =
    do System.IO.File.WriteAllText(path, editorText.Text)
  do editorText.TextChanged.Add update_text
  do editorText.Padding <- Thickness(10.0)
  do editorText.Margin <- Thickness(10.0)
  do editorText.AcceptsReturn <- true

  do base.Content <- editorText



type WPFWindow(samples, runDeduction) =
  inherit Window()

//  do base.Background <- new SolidColorBrush( Color.FromArgb(255uy,0uy,0uy,0uy) )
//  do base.Topmost <-  true
  do base.WindowStartupLocation <- WindowStartupLocation.CenterScreen

  do base.Height <- 550.0

  let stackPanel = new StackPanel()

  let upperLineStackPanel = new StackPanel()
  do upperLineStackPanel.Orientation <- Orientation.Horizontal
  do upperLineStackPanel.HorizontalAlignment <- HorizontalAlignment.Center

  let deductionList = new ComboBox()
  do deductionList.Width <- 225.0
  do for d in System.IO.Directory.GetDirectories(@".\Content") do deductionList.Items.Add(d) |> ignore
  let deductionListCachePath = "selectionCache.txt"
  do if System.IO.File.Exists deductionListCachePath then deductionList.SelectedIndex <- System.IO.File.ReadAllText deductionListCachePath |> System.Int32.Parse
  do deductionList.Margin <- Thickness(10.0)
  do deductionList.SelectedIndex <- 0

  let editRules = new Button()
  do editRules.Content <- "Edit rules"
  let oneditRules _ = (new RuleEditorWindow(deductionList.SelectedItem :?> string, "transform.mc")).Show() |> ignore
  do editRules.Click.Add oneditRules
  do editRules.Margin <- Thickness(10.0)

  let showHelp = new Button()
  do showHelp.Content <- "Show help"
  let onshowHelp _ = (new HelpWindow(samples)).Show() |> ignore
  do showHelp.Click.Add onshowHelp
  do showHelp.Margin <- Thickness(10.0)

  let showConsole = new Button()
  do showConsole.Content <- "Hide console"
  do showConsole.Click.Add
        (fun _ -> 
          let consoleHandle = GetConsoleWindow()
          let SW_HIDE = 0
          let SW_SHOW = 5
          ShowWindow(consoleHandle, SW_HIDE) |> ignore; showConsole.Visibility <- Visibility.Collapsed)
  do showConsole.Margin <- Thickness(10.0)

  do upperLineStackPanel.Children.Add deductionList |> ignore
  do upperLineStackPanel.Children.Add editRules |> ignore
  do upperLineStackPanel.Children.Add showHelp |> ignore
  do upperLineStackPanel.Children.Add showConsole |> ignore

  let programToRun = new TextBox()
  do programToRun.TextWrapping <- TextWrapping.Wrap
  do programToRun.FontFamily <- FontFamily("consolas")
  let cachePath() = 
    let currentRuleSet = (deductionList.SelectedItem :?> string).Trim([|'\\'; '.'|])
    currentRuleSet + ".cache.txt"
  let refreshProgramCache() =
    if System.IO.File.Exists (cachePath()) then 
      programToRun.Text <- System.IO.File.ReadAllText (cachePath())
    else
      let currentRuleSet = (deductionList.SelectedItem :?> string).Trim([|'\\'; '.'|])
      match samples |> Seq.tryFind (fun (r,s) -> r = currentRuleSet) with
      | Some (_,s) -> programToRun.Text <- s
      | _ -> programToRun.Text <- ""
  do refreshProgramCache()
  do programToRun.TextChanged.Add(fun _ -> System.IO.File.WriteAllText(cachePath(), programToRun.Text))
  do programToRun.AcceptsReturn <- true
  do programToRun.Height <- 200.0
  do programToRun.Width <- 700.0
  do programToRun.Padding <- Thickness(10.0)
  do programToRun.Margin <- Thickness(10.0)
  do deductionList.SelectionChanged.Add(fun _ -> refreshProgramCache(); System.IO.File.WriteAllText(deductionListCachePath, deductionList.SelectedIndex.ToString()))

  let deductionOutput = new TextBox()
  do deductionOutput.TextWrapping <- TextWrapping.Wrap
  do deductionOutput.FontFamily <- FontFamily("consolas")
  do deductionOutput.IsReadOnly <- true
  do deductionOutput.Height <- 170.0
  do deductionOutput.Width <- 700.0
  do deductionOutput.Padding <- Thickness(10.0)
  do deductionOutput.Margin <- Thickness(10.0)

  let runDeductionBtn = new Button()
  do runDeductionBtn.Content <- "Run deduction"
  let onRunDeduction _ = 
    deductionOutput.Text <- runDeduction (deductionList.SelectedItem :?> string) programToRun.Text
    ()
  do runDeductionBtn.Click.Add onRunDeduction
  do runDeductionBtn.Width <- 90.0
  do runDeductionBtn.Margin <- Thickness(10.0)

  do stackPanel.Children.Add upperLineStackPanel |> ignore
  do stackPanel.Children.Add programToRun |> ignore
  do stackPanel.Children.Add runDeductionBtn |> ignore
  do stackPanel.Children.Add deductionOutput |> ignore

  do base.Content <- stackPanel


let ShowGUI samples runDeduction =
  let w = new WPFWindow(samples, runDeduction)
  do w.ShowDialog() |> ignore
