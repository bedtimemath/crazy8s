using System.Text.Json;
using System.Text.Json.Serialization;

namespace C8S.Applications.Converters;

internal class JsonStringListConverter: JsonConverter<List<string>>
{
    public override List<string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => 
        reader.GetString()?.Split(',').ToList();

    public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options) =>
        writer.WriteStringValue(String.Join("'", value));

}