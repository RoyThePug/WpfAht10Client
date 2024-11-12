using System.Diagnostics;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Aht10.Tests.UITests;

public class BaseTest
{
    protected WindowsDriver<WindowsElement> Session;

    public BaseTest()
    {
        Process.Start(@"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe");
    }
    
    [SetUp]
    public void Setup()
    {
        var options = new DesiredCapabilities();
        options.SetCapability("app", "C:\\Users\\user\\source\\repos\\Wpf\\WpfAht10Client\\WpfAht10Client\\bin\\Debug\\net8.0-windows\\WpfAht10Client.exe");

        Session = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);
    }

    [TearDown]
    public void TearDown()
    {
        Session.Close();
    }

    protected void WaitForProgress()
    {
        var wait = new DefaultWait<WindowsDriver<WindowsElement>>(Session)
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        wait.Until(d =>
        {
            try
            {
                var progress = Session.FindElementByAccessibilityId("Progress");

                return !progress.Displayed;
            }
            catch (Exception)
            {
                return true;
            }
        });
    }
}