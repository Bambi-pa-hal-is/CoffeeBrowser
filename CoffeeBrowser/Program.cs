// Create a Playwright instance
using Microsoft.Playwright;

Console.WriteLine("Starting...");
Environment.SetEnvironmentVariable("DISPLAY", ":0");
using var playwright = await Playwright.CreateAsync();

var browserExecutablePath = "/home/pi/.cache/ms-playwright/chromium-1117/chrome-linux/chrome";

// Launch a new browser instance with the specified executable path
await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
{
    ExecutablePath = browserExecutablePath,
    Headless = false, // Set to true if you don't need a visible UI
    Args = new[]
                {
                    "--start-fullscreen",
                    "--disable-infobars",
                    "--start-maximized", 
                    "--disable-features=TranslateUI",
                    "--noerrdialogs",
                    "--kiosk" // This might help enforce fullscreen
                }
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
await page.Keyboard.PressAsync("F11");
Console.WriteLine("fullscreen requested");
// Refresh the page every hour
while (true)
{
    await Task.Delay(TimeSpan.FromHours(1));
    await page.ReloadAsync();
}