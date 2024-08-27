using System.Text.Json;
using System.Text.Json.Serialization;

namespace C8S.Applications.Converters;

internal class JsonNullableIntConverter: JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        (Int32.TryParse(reader.GetString(), out int number)) ? number : null;

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value?.ToString() ?? String.Empty);

}