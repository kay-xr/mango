using Terminal.Gui;

namespace Mango.Server;

public class Tui : Window
{
    public Tui(ServerManager serverManager)
    {
        var leftPane = new Window { Title = "Options", Width = Dim.Percent(25) };
        Add(leftPane);

        var startButton = new Button
        {
            Title = "Start Server",
            Y = 1,
            X = Pos.Center(),
            Width = Dim.Percent(100),
            ShadowStyle = ShadowStyle.None
        };
        var stopButton = new Button
        {
            Title = "Stop Server",
            Y = Pos.Bottom(startButton) + 1,
            X = Pos.Center(),
            Width = Dim.Percent(100),
            ShadowStyle = ShadowStyle.None
        };
        var restartButton = new Button
        {
            Title = "Restart Server",
            Y = Pos.Bottom(stopButton) + 1,
            X = Pos.Center(),
            Width = Dim.Percent(100),
            ShadowStyle = ShadowStyle.None
        };
        var updateButton = new Button
        {
            Title = "Update Server",
            Y = Pos.Bottom(restartButton) + 1,
            X = Pos.Center(),
            Width = Dim.Percent(100),
            ShadowStyle = ShadowStyle.None
        };

        startButton.Accepting += (sender, args) =>
        {
            Task.Run(async () =>
            {
                await serverManager.StartServerAsync();
                Application.Invoke(() => { MessageBox.Query("Info", "Server started", "OK"); });
            });
        };

        stopButton.Accepting += (sender, args) =>
        {
            Task.Run(async () =>
            {
                await serverManager.StopServerAsync();
                Application.Invoke(() => { MessageBox.Query("Info", "Server stopped", "OK"); });
            });
        };

        restartButton.Accepting += (sender, args) =>
        {
            Task.Run(async () =>
            {
                await serverManager.RestartServerAsync();
                Application.Invoke(() => { MessageBox.Query("Info", "Server restarted", "OK"); });
            });
        };

        leftPane.Add(startButton, stopButton, restartButton, updateButton);

        // console view
        var rightPane = new Window
        {
            Title = "Console",
            Width = Dim.Percent(75),
            X = Pos.Right(leftPane) + 1,
            Y = 0,
            Height = Dim.Fill()
        };
        Add(rightPane);

        var outputTextView = new TextView
        {
            ReadOnly = true,
            WordWrap = true,
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            Text = ""
        };
        rightPane.Add(outputTextView);

        serverManager.OutputDataReceived += line =>
        {
            Application.Invoke(() =>
            {
                outputTextView.Text += line + "\n";

                var textLines = outputTextView.Text.Split('\n');
                var visibleRows = outputTextView.Frame.Height;
                var newTopRow = Math.Max(0, textLines.Length - visibleRows);

                outputTextView.TopRow = newTopRow;
            });
        };
    }
}