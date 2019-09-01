using Newtonsoft.Json;
using System;

namespace ControllerWrapper
{
    public class BoolOrDoubleConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => writer.WriteValue(value);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            reader.ValueType == typeof(double) || reader.ValueType == typeof(float) ? (double)reader.Value : (bool)reader.Value ? 1 : 0;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(double);
        }
    }
}
