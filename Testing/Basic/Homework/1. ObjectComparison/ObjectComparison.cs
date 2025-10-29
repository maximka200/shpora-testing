using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    /*
     плюсы, относительно CheckCurrentTsar_WithCustomEquality
      - понятно, какое поле не совпадает
      - не нужно дописывать новые поля, при расширении Person
    */
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));
        
        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options.Excluding(info => info.Path.EndsWith("Id")));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    /*
     минусы:
     - при расширении Person нужно дописывать сравнение новых полей
     - непонятно, в каком месте и какое поле не проходит
     */
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
