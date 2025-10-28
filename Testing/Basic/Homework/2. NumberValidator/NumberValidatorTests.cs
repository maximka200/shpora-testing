
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    [TestCase(-1, 5, true, "precision must be a positive number")]
    [TestCase(2, -3, true, "precision must be a non-negative number less or equal than precision")]
    [TestCase(3, 3, true, "precision must be a non-negative number less or equal than precision")]
    public void Constructor_ДолжноВыкинутьArgumentException(int precision, int scale, bool onlyPositive,
        string errorText)
    {
        Action act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().Throw<ArgumentException>().WithMessage(errorText, $"precision: {precision}, scale: {scale}," +
                                                                       $" onlyPositive: {onlyPositive} must throw {errorText}");
    }

    [Test]
    [TestCase(2, 1, true)]
    public void Constructor_НеДолжноВыкинутьExceptions(int precision, int scale, bool onlyPositive)
    {
        var createNumValidator = () => new NumberValidator(precision, scale, onlyPositive);
        
        createNumValidator.Should().NotThrow<Exception>($"precision: {precision}, scale: {scale}," +
                                                         $" onlyPositive: {onlyPositive} should not throw exceptions");
    }   

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void IsValidNumber_ДолжноВернутьFalse_ЕслиСтрокаПустаяИлиNull(string? input)
    {
        var numValidator = new NumberValidator(1);

        var output = numValidator.IsValidNumber(input);

        output.Should().Be(false, $"input: '{input}' is null or empty, should return false");
    }

    [Test]
    [TestCase("--1.21", false)]
    [TestCase("1..21", false)]
    [TestCase("1,,21", false)]
    [TestCase(".2", false)]
    [TestCase("2.", false)]
    [TestCase("qwerty", false)]
    [TestCase("qwe.rty", false)]
    [TestCase("1", true)]
    public void IsValidNumber_ДолжноВернутьFalse_ЕслиСтрокаВНеправильномФормате(string input, bool flag)
    {
        var numValidator = new NumberValidator(5, 4, false);

        var output = numValidator.IsValidNumber(input);

        output.Should().Be(flag, $"input: {input} has invalid format, should return {flag}");
    }

    [Test]
    [TestCase("-123", false)]
    [TestCase("11.12", false)]
    [TestCase("-1.123", false)]
    [TestCase("11.123", false)]
    [TestCase("123", true)]
    public void IsValidNumber_ДолжноВернутьFalse_ЕслиЗнаковБольшеЧемPrecisionИлиЗнаковПослеЗапятойБольшеЧемScale(string input, bool flag)
    {
        var numValidator = new NumberValidator(3, 2, false);

        var output = numValidator.IsValidNumber(input);

        output.Should().Be(flag, "input: {input} exceeds precision or scale, should return {flag}");
    }

    [Test]
    [TestCase("-1", false)]
    [TestCase("-1.21", false)]
    [TestCase("-1,11", false)]
    [TestCase("123", true)]
    public void IsValidNumber_ДолжноВернутьFalse_ЕслиOnlyPositiveЭтоTrueАЧислоОтрицательное(string input, bool flag)
    {
        var numValidator = new NumberValidator(3, 2, true);

        var output = numValidator.IsValidNumber(input);

        output.Should().Be(flag, "input: {input} is negative while onlyPositive is true, should return {flag}");
    }

    [Test]
    [TestCase("1111", 5, 0, true)]
    [TestCase("11111", 5, 0, true)]
    [TestCase("111111", 5, 0, false)]
    [TestCase("11.11", 4, 2, true)]
    [TestCase("11,33", 4, 2, true)]
    [TestCase("13.333", 5, 2, false)]
    [TestCase("-11.33", 5, 2, false)]
    public void IsValidNumber_ДолжноВернутьFalse_ЕслиВыходЗаПределыPrecisionИлиScale(string input, int precision, int scale, bool expected)
    {
        var validator = new NumberValidator(precision, scale, true);
        
        validator.IsValidNumber(input).Should().Be(expected, "input: {input}, precision: {precision}, scale: {scale} should return {expected}");
    }
}