using System.Text.Json;
using System.Text.Json.Serialization;

namespace C8S.Applications.Converters;

internal class JsonNullableStringConverter: JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        String.IsNullOrEmpty(reader.GetString()) ? null : reader.GetString();

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value?.ToString() ?? String.Empty);

}