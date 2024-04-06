using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

namespace AnkiPoetry.Engine;

public static class CsvSaver
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Card))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DefaultClassMap<Card>))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MemberMap<Card, string>))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CsvHelper.Expressions.RecordManager))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CsvHelper.Expressions.RecordWriterFactory))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CsvHelper.Expressions.RecordCreatorFactory))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CsvHelper.Expressions.RecordHydrator))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CsvHelper.Expressions.ExpressionManager))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CsvHelper.TypeConversion.StringConverter))]
    public static string CreateCsv<T>(IEnumerable<T> cards, string[] infos)
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = false,
            ShouldQuote = args => true,
        };

        using var writer = new StringWriter();

        foreach (var info in infos)
            writer.WriteLine(info);

        using var csv = new CsvWriter(writer, configuration);
        csv.WriteRecords(cards);

        return writer.ToString();
    }
}
