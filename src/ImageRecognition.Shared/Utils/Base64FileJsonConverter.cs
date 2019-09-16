using System;
using Newtonsoft.Json;

namespace ImageRecognition.Shared.Utils
{
    public class Base64FileJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) 
            => throw new NotImplementedException();

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) 
            => Convert.FromBase64String(reader.Value as string);

        public override bool CanConvert(Type objectType) 
            => objectType == typeof(string);
    }
}