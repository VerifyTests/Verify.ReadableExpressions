public class Tests
{
    #region Example

    [Fact]
    public Task VerifySalaryCalculation()
    {
        var expression = ComplexExpressionTrees.SalaryCalculation();
        return Verify(expression);
    }

    #endregion

    [Fact]
    public Task VerifyNullSafeAccess()
    {
        var expression = ComplexExpressionTrees.NullSafeAccess();
        return Verify(expression);
    }

    [Fact]
    public Task VerifyStringManipulation()
    {
        var expression = ComplexExpressionTrees.StringManipulation();
        return Verify(expression);
    }

    [Fact]
    public Task VerifySalaryCalculationNested()
    {
        var expression = ComplexExpressionTrees.SalaryCalculation();
        return Verify(new
        {
            expression
        });
    }

    [Fact]
    public Task VerifyNullSafeAccessNested()
    {
        var expression = ComplexExpressionTrees.NullSafeAccess();
        return Verify(new
        {
            expression
        });
    }

    [Fact]
    public Task VerifyStringManipulationNested()
    {
        var expression = ComplexExpressionTrees.StringManipulation();
        return Verify(new
        {
            expression
        });
    }

    static readonly Person[] testPeople =
    [
        new("John", "Smith", 25, "New York", 55000m, true),
        new("Jane", "Doe", 45, "New Jersey", 75000m, true),
        new("Bob", "Wilson", 70, "Boston", 40000m, false),
        new("Al", "Brown", 22, null, 30000m, true),
        new("Sarah", "Johnson", 35, "Newark", 60000m, false)
    ];

    [Fact]
    public void SalaryCalculation_ShouldCompile()
    {
        // Arrange & Act
        var expression = ComplexExpressionTrees.SalaryCalculation();
        var compiled = expression.Compile();

        // Assert
        Assert.NotNull(compiled);
        Assert.Equal(typeof(decimal), expression.ReturnType);
    }

    [Theory]
    [InlineData("John", "Smith", 63250)] // Age 25 < 30: $55,000 * 1.15
    [InlineData("Jane", "Doe", 81000)] // Age 45 (30-50): $75,000 * 1.08
    [InlineData("Bob", "Wilson", 41200)] // Age 70 > 50: $40,000 * 1.03
    [InlineData("Al", "Brown", 34500)] // Age 22 < 30: $30,000 * 1.15
    [InlineData("Sarah", "Johnson", 64800)] // Age 35 (30-50): $60,000 * 1.08
    public void SalaryCalculation_ShouldCalculateCorrectBonus(string firstName, string lastName, decimal expectedSalary)
    {
        // Arrange
        var expression = ComplexExpressionTrees.SalaryCalculation();
        var compiled = expression.Compile();
        var person = testPeople.First(p => p.FirstName == firstName && p.LastName == lastName);

        // Act
        var result = compiled(person);

        // Assert
        Assert.Equal(expectedSalary, result);
    }

    [Fact]
    public void StringManipulation_ShouldCompile()
    {
        // Arrange & Act
        var expression = ComplexExpressionTrees.StringManipulation();
        var compiled = expression.Compile();

        // Assert
        Assert.NotNull(compiled);
        Assert.Equal(typeof(string), expression.ReturnType);
    }

    [Theory]
    [InlineData("John", "Smith", "NEW YORK - Smi")]
    [InlineData("Jane", "Doe", "NEW JERSEY - Doe")]
    [InlineData("Bob", "Wilson", "BOSTON - Wil")]
    [InlineData("Al", "Brown", "UNKNOWN - Bro")]
    [InlineData("Sarah", "Johnson", "NEWARK - Joh")]
    public void StringManipulation_ShouldFormatCorrectly(string firstName, string lastName, string expected)
    {
        // Arrange
        var expression = ComplexExpressionTrees.StringManipulation();
        var compiled = expression.Compile();
        var person = testPeople.First(p => p.FirstName == firstName && p.LastName == lastName);

        // Act
        var result = compiled(person);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void NullSafeAccess_ShouldCompile()
    {
        // Arrange & Act
        var expression = ComplexExpressionTrees.NullSafeAccess();
        var compiled = expression.Compile();

        // Assert
        Assert.NotNull(compiled);
        Assert.Equal(typeof(string), expression.ReturnType);
    }

    [Theory]
    [InlineData("John", "Smith", "n")] // New York -> 'n'
    [InlineData("Jane", "Doe", "n")] // New Jersey -> 'n'
    [InlineData("Bob", "Wilson", "b")] // Boston -> 'b'
    [InlineData("Al", "Brown", "unknown")] // null -> "unknown"
    [InlineData("Sarah", "Johnson", "n")] // Newark -> 'n'
    public void NullSafeAccess_ShouldHandleNullAndExtractFirstChar(string firstName, string lastName, string expected)
    {
        // Arrange
        var expression = ComplexExpressionTrees.NullSafeAccess();
        var compiled = expression.Compile();
        var person = testPeople.First(p => p.FirstName == firstName && p.LastName == lastName);

        // Act
        var result = compiled(person);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SalaryCalculation_ShouldContainConditionalExpressions()
    {
        // Arrange
        var expression = ComplexExpressionTrees.SalaryCalculation();

        // Act & Assert - verify nested conditionals
        Assert.Equal(ExpressionType.Conditional, expression.Body.NodeType);
        Assert.IsAssignableFrom<ConditionalExpression>(expression.Body);

        var conditional = (ConditionalExpression) expression.Body;
        Assert.Equal(ExpressionType.Conditional, conditional.IfFalse.NodeType); // Nested conditional
    }
}