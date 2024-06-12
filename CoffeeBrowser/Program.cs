// Create a Playwright instance
using Microsoft.Playwright;

using var playwright = await Playwright.CreateAsync();

var browserExecutablePath = "/home/pi/.cache/ms-playwright/chromium-1117/chrome-linux/chrome";

// Launch a new browser instance with the specified executable path
await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
{
    ExecutablePath = browserExecutablePath,
    Headless = true // Set to true if you don't need a visible UI
});

// Create a new browser context
var context = await browser.NewContextAsync(new BrowserNewContextOptions
{
    ViewportSize = null // Setting this to null makes the browser fullscreen
});

// Create a new page
var page = await context.NewPageAsync();

// Navigate to the specified URL
await page.GotoAsync("https://kaffe.kosatupp.se");

// Enter fullscreen mode
await page.EvaluateAsync("() => document.documentElement.requestFullscreen()");

// Refresh the page every hour
while (true)
{
    await Task.Delay(TimeSpan.FromHours(1));
    await page.ReloadAsync();
}