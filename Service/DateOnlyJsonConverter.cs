using System.Text.Json.Serialization;
using System.Text.Json;

namespace Service
{

        public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
        {
            #region Methods

            public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                try
                {
                    return DateOnly.FromDateTime(reader.GetDateTime());
                }
                catch (Exception ex)
                {
                    return DateOnly.FromDateTime(DateTime.Now);
                }

            }

            public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            {
                var isoDate = value.ToString("O");
                writer.WriteStringValue(isoDate);
            }

            #endregion
        }
    
}
