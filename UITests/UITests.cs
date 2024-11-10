
namespace UITests;

public class UITests : BaseTest
{
    [Test]
    public void CheckConnectionTest()
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
    public void PlottingTabTest()
    {
        WaitForProgress();
        
        Thread.Sleep(1000);
        Session.FindElementByAccessibilityId("Plotting").Click();
        
        Assert.IsTrue(Session.FindElementByAccessibilityId("Plot").Displayed);
        
        Thread.Sleep(2000);
    }
}