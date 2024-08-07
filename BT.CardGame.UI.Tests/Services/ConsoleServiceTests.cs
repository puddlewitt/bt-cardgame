using BT.CardGame.UI.Services;

namespace BT.CardGame.UI.Tests.Services;

public class ConsoleServiceTests
{
    private const string SomeErrorOutput = "SOME_ERROR_OUTPUT";
    private const string SomeOutput = "SOME_OUTPUT";
    private const string SomeInput = "SOME_INPUT";

    private ConsoleService _consoleService;

    [SetUp]
    public void SetUp()
    {
        _consoleService = new ConsoleService();
    }

    [Test]
    public void WriteLine_ShouldWriteToOutput_WhenOutputUsedToInformUserOfAnErrorMessage()
    {
        using var sw = new StringWriter();
        
        Console.SetOut(sw);

        _consoleService.WriteLine(new Exception(SomeErrorOutput));
        
        var response = sw.GetStringBuilder().ToString();
        
        Assert.That(response, Is.EqualTo($"System.Exception: {SomeErrorOutput}{Environment.NewLine}"));
    }
    
    [Test]
    public void WriteLine_ShouldWriteToOutput_WhenOutputUsedToInformUserOfAMessage()
    {

        using var sw = new StringWriter();
        
        Console.SetOut(sw);

        _consoleService.WriteLine(SomeOutput);
        
        var response = sw.GetStringBuilder().ToString();
        
        Assert.That(response, Is.EqualTo($"{SomeOutput}{Environment.NewLine}"));
    }
    
    [Test]
    public void ReadLine_ShouldReadFromInput_WhenInputProvided()
    {

        using var sr = new StringReader(SomeInput);
        
        Console.SetIn(sr);
        
        var response = _consoleService.ReadLine();
        
        Assert.That(response, Is.EqualTo(SomeInput));
    }
}