namespace WiredBrainCoffee.DataProcessor.Parsing;

public class CsvLineParserTests
{
    [Fact]
    public void ShouldParseValidLine()
    {
        string[] csvLines = ["Cappuccino;10/27/2022 8:06:04 AM"];

        var machineDataItems = CsvLineParser.Parse(csvLines);

        Assert.NotNull(machineDataItems);

        var (coffeeType, createdAt) = Assert.Single(machineDataItems);

        Assert.Equal("Cappuccino", coffeeType);
        Assert.Equal(new DateTime(2022, 10, 27, 8, 6, 4, DateTimeKind.Unspecified), createdAt);
    }

    [Fact]
    public void ShouldSkipEmptyLines()
    {
        string[] csvLines = ["", " "];

        var machineDataItems = CsvLineParser.Parse(csvLines);

        Assert.NotNull(machineDataItems);
        Assert.Empty(machineDataItems);
    }

    [InlineData("Cappuccino", "Invalid csv line: Missing separator in")]
    [InlineData("Cappuccino;10/27/2022 8:06:04 AM;Friday", "Invalid csv line: Multiple separator in")]
    [InlineData("Cappuccino;InvalidDateTime", "Invalid datetime in csv line")]
    [Theory]
    public void ShouldThrowExceptionForInvalidLine(string csvLine, string expectedMessagePrefix)
    {
        string[] csvLines = [csvLine];

        var exception = Assert.Throws<Exception>(() => CsvLineParser.Parse(csvLines));

        Assert.Equal($"{expectedMessagePrefix}: {csvLine}", exception.Message);
    }
}