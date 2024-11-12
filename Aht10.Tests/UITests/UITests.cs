namespace Aht10.Tests.UITests;

public class UITests : BaseTest
{
    [Test]
    public void Check_сonnection()
    {
        WaitForProgress();

        Thread.Sleep(1000);
        Session.FindElementByAccessibilityId("ConnectBtn").Click();
        Thread.Sleep(1000);
        Assert.IsTrue(Session.FindElementByAccessibilityId("DisconnectBtn").Displayed);
        Session.FindElementByAccessibilityId("DisconnectBtn").Click();
        Assert.IsTrue(Session.FindElementByAccessibilityId("ConnectBtn").Displayed);
        
        Thread.Sleep(2000);
    }
    
    [Test]
    public void Check_measurements_exist()
    {
        WaitForProgress();

        Thread.Sleep(1000);
        Session.FindElementByAccessibilityId("ConnectBtn").Click();
        Thread.Sleep(1000);
        Assert.IsTrue(Session.FindElementByAccessibilityId("DisconnectBtn").Displayed);
        Session.FindElementByAccessibilityId("DisconnectBtn").Click();
        Assert.IsTrue(Session.FindElementByAccessibilityId("ConnectBtn").Displayed);

        var measurementItems = Session.FindElementByAccessibilityId("MeasurementDataGrid").FindElementsByClassName("DataGridRow"); 
        
        Assert.IsTrue(measurementItems.Any());
        
        Thread.Sleep(2000);
    }

    [Test]
    public void Plotting_tab_save_image()
    {
        WaitForProgress();
        
        Thread.Sleep(1000);
        
        Session.FindElementByAccessibilityId("Plotting").Click();
        
        Assert.IsTrue(Session.FindElementByAccessibilityId("Plot").Displayed);
        Assert.IsTrue(Session.FindElementByAccessibilityId("SaveImageBtn").Displayed);
        
        Session.FindElementByAccessibilityId("SaveImageBtn").Click();
        
        WaitForProgress();
        
        Thread.Sleep(2000);
    }
}