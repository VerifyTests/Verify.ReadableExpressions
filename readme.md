# <img src="/src/icon.png" height="30px"> Verify.ReadableExpressions

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://ci.appveyor.com/api/projects/status/1b88m1705v9i8l3m?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-ReadableExpressions)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.ReadableExpressions.svg)](https://www.nuget.org/packages/Verify.ReadableExpressions/)

Adds [Verify](https://github.com/VerifyTests/Verify) support for Expression Trees using [ReadableExpressions](https://github.com/agileobjects/ReadableExpressions).<!-- singleLineInclude: intro. path: /docs/intro.include.md -->

**See [Milestones](../../milestones?state=closed) for release notes.**


## Sponsors


### Entity Framework Extensions<!-- include: zzz. path: /docs/zzz.include.md -->

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.ReadableExpressions) is a major sponsor and is proud to contribute to the development this project.

[![Entity Framework Extensions](https://raw.githubusercontent.com/VerifyTests/Verify.ReadableExpressions/refs/heads/main/docs/zzz.png)](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.ReadableExpressions)<!-- endInclude -->


## NuGet

 * https://nuget.org/packages/Verify.ReadableExpressions


## Usage


### Enable

<!-- snippet: enable -->
<a id='snippet-enable'></a>
```cs
[ModuleInitializer]
public static void Init() =>
    VerifyReadableExpressions.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Sample Expression

Given a complex Expression:

<!-- snippet: SampleExpression -->
<a id='snippet-SampleExpression'></a>
```cs
// Complex calculation with conditional logic
// Equivalent to: p => p.Age < 30
//                     ? p.Salary * 1.15m
//                     : (p.Age < 50
//                         ? p.Salary * 1.08m
//                         : p.Salary * 1.03m)
public static Expression<Func<Person, decimal>> SalaryCalculation()
{
    var param = Expression.Parameter(typeof(Person));
    var ageProperty = Expression.Property(param, nameof(Person.Age));
    var salaryProperty = Expression.Property(param, nameof(Person.Salary));

    // Nested conditional: age-based salary multipliers
    var youngBonus = Expression.Multiply(
        salaryProperty,
        Expression.Constant(1.15m));
    var midBonus = Expression.Multiply(
        salaryProperty,
        Expression.Constant(1.08m));
    var seniorBonus = Expression.Multiply(
        salaryProperty,
        Expression.Constant(1.03m));

    var ageLessThan30 = Expression.LessThan(
        ageProperty,
        Expression.Constant(30));
    var ageLessThan50 = Expression.LessThan(
        ageProperty,
        Expression.Constant(50));

    var innerConditional = Expression.Condition(
        ageLessThan50,
        midBonus,
        seniorBonus);
    var outerConditional = Expression.Condition(
        ageLessThan30,
        youngBonus,
        innerConditional);

    return Expression.Lambda<Func<Person, decimal>>(outerConditional, param);
}
```
<sup><a href='/src/Tests/ComplexExpressionTrees.cs#L5-L47' title='Snippet source file'>snippet source</a> | <a href='#snippet-SampleExpression' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Test

<!-- snippet: Example -->
<a id='snippet-Example'></a>
```cs
[Fact]
public Task VerifySalaryCalculation()
{
    var expression = ComplexExpressionTrees.SalaryCalculation();
    return Verify(expression);
}
```
<sup><a href='/src/Tests/Tests.cs#L3-L12' title='Snippet source file'>snippet source</a> | <a href='#snippet-Example' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Resulting

<!-- snippet: Tests.VerifySalaryCalculation.verified.txt -->
<a id='snippet-Tests.VerifySalaryCalculation.verified.txt'></a>
```txt
person => (person.Age < 30)
  ? person.Salary * 1.15m
  : (person.Age < 50) ? person.Salary * 1.08m : person.Salary * 1.03m
```
<sup><a href='/src/Tests/Tests.VerifySalaryCalculation.verified.txt#L1-L3' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.VerifySalaryCalculation.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Icon

[Tree](https://thenounproject.com/term/tree/8100408/) designed by [BnB Studio](https://thenounproject.com/creator/bnbstudio/) from [The Noun Project](https://thenounproject.com/).
