using BT.CardGame.UI.Services;

namespace BT.CardGame.UI.Tests.Services;

public class ConsoleServiceTests
{
    private ConsoleService _consoleService;

    [SetUp]
    public void SetUp()
    {
        _consoleService = new ConsoleService();
    }

    [Test]
    public void ShouldNotThrow_Wheninvoked()
    {
        Assert.That(() => _consoleService.WriteLine("A_TEST"), Throws.Nothing);
        Assert.That(() => _consoleService.WriteLine(new Exception("OH_NO")), Throws.Nothing);
    }
}