class ExpressionConverter :
    WriteOnlyJsonConverter<Expression>
{
    public override void Write(VerifyJsonWriter writer, Expression value)
    {
        var result = Convert(value);
        writer.WriteValue(result);
    }

    public static string Convert(Expression value) =>
        value.ToReadableString(
            _ => _
                .UseExplicitTypeNames
                .UseExplicitGenericParameters
                .ShowCapturedValues
                .IndentUsing("  "));
}