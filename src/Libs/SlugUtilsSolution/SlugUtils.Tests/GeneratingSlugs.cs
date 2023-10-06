using NSubstitute;

namespace SlugUtils.Tests;
public class GeneratingSlugs
{
    [Theory]
    [InlineData("Excel Goes Boom", "excel-goes-boom")]
    [InlineData("Tacos are dElicious", "tacos-are-delicious")]
    [InlineData("  birds  ", "birds")]
    [InlineData("dog % $ cat", "dog-cat")]

    public async Task CanGenerateSlugs(string input, string expected)
    {
        var slugGenerator = new SlugGenerator();

        string result = await slugGenerator.GenerateSlugAsync(input, _ => Task.FromResult(true));

        Assert.Equal(expected, result);
    }

    [Theory]

    [InlineData("Excel Goes Boom", "excel-goes-boom-a")]
    [InlineData("Tacos Are Good", "tacos-are-good-z")]
    public async Task SlugsMustBeUnique(string input, string expected)
    {
        var slugGenerator = new SlugGenerator();

        var cb = Substitute.For<Func<string, Task<bool>>>();
        cb("excel-goes-boom").Returns(false);
        cb("excel-goes-boom-a").Returns(true);
        cb("tacos-are-good-z").Returns(true);

        string result = await slugGenerator.GenerateSlugAsync(input, cb);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task NoUniqueAvailableAppendsGuid()
    {
        var slugGenerator = new SlugGenerator();

        var cb = Substitute.For<Func<string, Task<bool>>>();
        // NEVER returning true - for all your attempts.
        string result = await slugGenerator.GenerateSlugAsync("pizza", cb);

        Assert.StartsWith("pizza", result);
        var aGuidIsThisLong = Guid.NewGuid().ToString().Length;
        Assert.Equal(5 + aGuidIsThisLong, result.Length);

    }
}
