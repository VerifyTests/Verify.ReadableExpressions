namespace VerifyTests;

public static class VerifyReadableExpressions
{
    public static bool Initialized { get; private set; }

    public static void Initialize()
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();
        VerifierSettings
            .AddExtraSettings(_ =>
            {
                var converters = _.Converters;
                converters.Insert(0, new ExpressionConverter());
            });
        VerifierSettings.TreatAsString<Expression>((target, _) => ExpressionConverter.Convert(target), true);
    }
}