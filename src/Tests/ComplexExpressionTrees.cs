public record Person(string FirstName, string LastName, int Age, string? City, decimal Salary, bool IsActive);

public static class ComplexExpressionTrees
{
    #region SampleExpression
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
    #endregion

    // Complex string manipulation with multiple method calls and null coalescing
    // Equivalent to: p => (p.City ?? "Unknown").ToUpper().Trim() + " - " +
    //                     p.LastName.Substring(0, Math.Min(3, p.LastName.Length))
    public static Expression<Func<Person, string>> StringManipulation()
    {
        var param = Expression.Parameter(typeof(Person));

        // (p.City ?? "Unknown")
        var cityProperty = Expression.Property(param, nameof(Person.City));
        var cityCoalesce = Expression.Coalesce(cityProperty, Expression.Constant("Unknown"));

        // .ToUpper().Trim()
        var toUpperMethod = typeof(string).GetMethod(nameof(string.ToUpper), Type.EmptyTypes)!;
        var trimMethod = typeof(string).GetMethod(nameof(string.Trim), Type.EmptyTypes)!;
        var cityUpper = Expression.Call(cityCoalesce, toUpperMethod);
        var cityTrimmed = Expression.Call(cityUpper, trimMethod);

        // p.LastName.Substring(0, Math.Min(3, p.LastName.Length))
        var lastNameProperty = Expression.Property(param, nameof(Person.LastName));
        var lastNameLength = Expression.Property(lastNameProperty, nameof(string.Length));
        var mathMinMethod = typeof(Math).GetMethod(nameof(Math.Min), [typeof(int), typeof(int)])!;
        var minLength = Expression.Call(null, mathMinMethod, Expression.Constant(3), lastNameLength);
        var substringMethod = typeof(string).GetMethod(nameof(string.Substring), [typeof(int), typeof(int)])!;
        var lastNameSubstring = Expression.Call(lastNameProperty, substringMethod, Expression.Constant(0), minLength);

        // Concatenation: cityTrimmed + " - " + lastNameSubstring
        var concatMethod = typeof(string).GetMethod(nameof(string.Concat), [typeof(string), typeof(string), typeof(string)])!;
        var result = Expression.Call(null, concatMethod, cityTrimmed, Expression.Constant(" - "), lastNameSubstring);

        return Expression.Lambda<Func<Person, string>>(result, param);
    }

    // Complex expression with try-catch-like behavior using conditionals
    // Equivalent to: p => p.City != null && p.City.Length > 0
    //                     ? p.City[0].ToString().ToLower()
    //                     : "unknown"
    public static Expression<Func<Person, string>> NullSafeAccess()
    {
        var param = Expression.Parameter(typeof(Person));
        var cityProperty = Expression.Property(param, nameof(Person.City));

        // Check: p.City != null && p.City.Length > 0
        var cityNotNull = Expression.NotEqual(cityProperty, Expression.Constant(null, typeof(string)));
        var cityLength = Expression.Property(cityProperty, nameof(string.Length));
        var lengthCheck = Expression.GreaterThan(cityLength, Expression.Constant(0));
        var safetyCheck = Expression.AndAlso(cityNotNull, lengthCheck);

        // True branch: p.City[0].ToString().ToLower()
        var indexer = typeof(string).GetProperty("Chars")!;
        var firstChar = Expression.MakeIndex(cityProperty, indexer, [Expression.Constant(0)]);
        var toStringMethod = typeof(char).GetMethod(nameof(ToString), Type.EmptyTypes)!;
        var charToString = Expression.Call(firstChar, toStringMethod);
        var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
        var lowerCase = Expression.Call(charToString, toLowerMethod);

        // False branch: "unknown"
        var fallback = Expression.Constant("unknown");

        var conditional = Expression.Condition(safetyCheck, lowerCase, fallback);

        return Expression.Lambda<Func<Person, string>>(conditional, param);
    }
}