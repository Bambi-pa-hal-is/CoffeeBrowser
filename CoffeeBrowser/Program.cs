// Create a Playwright instance
using Microsoft.Playwright;
using System.Diagnostics;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Starting...");
        Environment.SetEnvironmentVariable("DISPLAY", ":0");
        using var playwright = await Playwright.CreateAsync();

        var browserExecutablePath = "/home/pi/.cache/ms-playwright/chromium-1117/chrome-linux/chrome";

        // Launch a new browser instance with the specified executable path
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            ExecutablePath = browserExecutablePath,
            Headless = false, // Set to true if you don't need a visible UI
            Args = new[] { "--start-maximized", "--disable-infobars" } // Start maximized
        });

        Console.WriteLine("browser created");

        // Create a new browser context
        //int screenWidth = 480;  // Set your screen width here
        //int screenHeight = 320; // Set your screen height here

        // Create a new browser context with the screen resolution
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = ViewportSize.NoViewport
        });
        Console.WriteLine("context created");


        // Create a new page
        var page = await context.NewPageAsync();
        Console.WriteLine("new page created");

        // Navigate to the specified URL
        await page.GotoAsync("https://kaffe.kosatupp.se/coffeemachine");
        Console.WriteLine("go to...");
        await page.ContentAsync();
        // Enter fullscreen mode
        // Wait for the button with id fullscreenButton and click it
        await page.WaitForSelectorAsync("#fullscreenButton");
        await page.ClickAsync("#fullscreenButton");
        Console.WriteLine("fullscreen button clicked");
        // Wait for the page to load completely
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Console.WriteLine("page loaded");

        // Wait for an additional second to ensure everything is ready
        await Task.Delay(500);

        await page.Keyboard.PressAsync("F11");
        Console.WriteLine("fullscreen requested");
        await Task.Delay(500);
        await page.EvaluateAsync("() => { document.documentElement.requestFullscreen().catch(console.error); }");
        await Task.Delay(1000);
        if(OperatingSystem.IsLinux())
        {
            SimulateKeyPress("F11");
        }

        // Refresh the page every hour
        while (true)
        {
            await Task.Delay(TimeSpan.FromHours(1));
            await page.ReloadAsync();
        }
    }

    public static void SimulateKeyPress(string key)
    {
        try
        {
            // Create a new process to run the xdotool command
            Process process = new Process();
            process.StartInfo.FileName = "xdotool";
            process.StartInfo.Arguments = $"key {key}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            // Start the process
            process.Start();

            // Read the output (if needed)
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            // Wait for the process to exit
            process.WaitForExit();

            // Optionally, print the output for debugging
            if (!string.IsNullOrEmpty(output))
            {
                Console.WriteLine("Output: " + output);
            }
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("Error: " + error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
    }
}