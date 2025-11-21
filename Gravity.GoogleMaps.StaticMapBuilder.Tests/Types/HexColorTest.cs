namespace Gravity.GoogleMaps.StaticMapBuilder.Tests.Types;

[TestSubject(typeof(HexColor))]
public class HexColorTest
{

    [Theory]
    [InlineData("0xFFFFFF")]
    [InlineData("0x000000")]
    [InlineData("0xABCDEF")]
    [InlineData("0xFF00FF88")]
    public void Ctor_ValidHex_DoesNotThrow(string input)
    {
        HexColor color = new(input);
        Assert.Equal(input, color.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("0x123")]
    [InlineData("0xGHIJKL")]
    [InlineData("123456")]
    [InlineData("0x123456789")]
    public void Ctor_InvalidHex_Throws(string input)
    {
        Assert.Throws<ArgumentException>(() => new HexColor(input));
    }

    [Theory]
    [InlineData("0xFFFFFF", false)]
    [InlineData("0xFFFFFF00", true)]
    [InlineData("0xFFFFFF88", true)]
    [InlineData("0xFFFFFFff", false)]
    public void IsAlphaSet_ReturnsCorrectValue(string input, bool expected)
    {
        HexColor color = new(input);
        Assert.Equal(expected, color.IsAlphaSet);
    }

    [Fact]
    public void ImplicitOperator_CreatesInstance()
    {
        HexColor color = "0xABCDEF";
        Assert.Equal("0xABCDEF", color.ToString());
    }

    [Theory]
    [InlineData("0xFFFFFF")]
    [InlineData("0x123456")]
    [InlineData("0xABCDEF99")]
    public void ToString_ReturnsOriginalInput(string input)
    {
        HexColor color = new(input);
        Assert.Equal(input, color.ToString());
    }
}