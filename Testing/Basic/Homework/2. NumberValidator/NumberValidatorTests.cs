using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    [TestCase(-1, 5, true, "precision must be a positive number")]
    [TestCase(2, -3, true, "scale must be a non-negative number less than precision")]
    [TestCase(3, 3, true, "scale must be a non-negative number less than precision")]
    public void Constructor_ShouldThrowArgumentException_WhenPrecisionOrScaleAreInvalid(
        int precision, int scale, bool onlyPositive, string errorText)
    {
        var createSut = () => new NumberValidator(precision, scale, onlyPositive);
        
        createSut.Should().Throw<ArgumentException>().WithMessage(errorText);
    }

    [Test]
    [TestCase(2, 1, true)]
    public void Constructor_ShouldNotThrowExceptions_WhenArgumentsAreValid(int precision, int scale, bool onlyPositive)
    {
        var createSut = () => new NumberValidator(precision, scale, onlyPositive);
        
        createSut.Should().NotThrow<Exception>();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void IsValidNumber_ShouldReturnFalse_WhenInputIsNullOrEmpty(string? input)
    {
        var sut = new NumberValidator(1);

        var actualOutput = sut.IsValidNumber(input);

        actualOutput.Should().BeFalse($"input: '{input}' is null or empty, should return false");
    }

    [Test]
    [TestCase("--1.21")]
    [TestCase("1..21")]
    [TestCase("1,,21")]
    [TestCase(".2")]
    [TestCase("2.")]
    [TestCase("qwerty")]
    [TestCase("qwe.rty")]
    public void IsValidNumber_ShouldReturnFalse_WhenInputHasInvalidFormat(string input)
    {
        var sut = new NumberValidator(5, 4);

        var actualOutput = sut.IsValidNumber(input);

        actualOutput.Should().BeFalse($"input '{input}' has invalid format, should return false");
    }

    [Test]
    [TestCase("1")]
    [TestCase("+1")]
    [TestCase("-0.7")]
    [TestCase("123,45")]
    public void IsValidNumber_ShouldReturnTrue_WhenInputHasValidFormat(string input)
    {
        var sut = new NumberValidator(5, 4);

        var actualOutput = sut.IsValidNumber(input);

        actualOutput.Should().BeTrue($"input '{input}' has valid format, should return true");
    }

    [Test]
    [TestCase("-123")]
    [TestCase("11.12")]
    [TestCase("-1.123")]
    [TestCase("11.123")]
    public void IsValidNumber_ShouldReturnFalse_WhenInputExceedsPrecisionOrScale(string input)
    {
        var sut = new NumberValidator(3, 2);

        var actualOutput = sut.IsValidNumber(input);

        actualOutput.Should().BeFalse($"input '{input}' exceeds precision or scale, should return false");
    }

    [Test]
    [TestCase("123")]
    [TestCase("1.2")]
    [TestCase("0.12")]
    public void IsValidNumber_ShouldReturnTrue_WhenInputWithinPrecisionAndScaleLimits(string input)
    {
        var sut = new NumberValidator(3, 2);

        var actualOutput = sut.IsValidNumber(input);

        actualOutput.Should().BeTrue($"input '{input}' fits within precision and scale limits, should return true");
    }

    [Test]
    [TestCase("-1")]
    [TestCase("-1.21")]
    [TestCase("-1,11")]
    public void IsValidNumber_ShouldReturnFalse_WhenOnlyPositiveIsTrueAndInputIsNegative(string input)
    {
        var sut = new NumberValidator(3, 2, true);

        var actualOutput = sut.IsValidNumber(input);

        actualOutput.Should().BeFalse($"input '{input}' is negative while onlyPositive is true, should return false");
    }

    [Test]
    [TestCase("1111", 5, 0)]
    [TestCase("11111", 5, 0)]
    [TestCase("11.11", 4, 2)]
    [TestCase("11,33", 4, 2)]
    public void IsValidNumber_ShouldReturnTrue_WhenInputFitsGivenPrecisionAndScale(string input, int precision, int scale)
    {
        var sut = new NumberValidator(precision, scale, true);
    
        var actualOutput = sut.IsValidNumber(input);

        actualOutput.Should().BeTrue($"input '{input}' fits into precision {precision} and scale {scale}, should return true");
    }

    [Test]
    [TestCase("111111", 5, 0)]
    [TestCase("13.333", 5, 2)]
    [TestCase("-11.33", 5, 2)]
    public void IsValidNumber_ShouldReturnFalse_WhenInputExceedsGivenPrecisionOrScale(string input, int precision, int scale)
    {
        var sut = new NumberValidator(precision, scale, true);
    
        var actualOutput = sut.IsValidNumber(input);

        actualOutput.Should().BeFalse($"input '{input}' exceeds precision {precision} or scale {scale}, should return false");
    }

    [Test]
    [TestCase("-11.33", 5, 2, false)]
    public void IsValidNumber_ShouldReturnFalse_WhenNumberIsNegativeAndOnlyPositiveTrue(string input, int precision, int scale, bool expectedOutput)
    {
        var sut = new NumberValidator(precision, scale, true);
        
        sut.IsValidNumber(input).Should().Be(expectedOutput);
    }
}
