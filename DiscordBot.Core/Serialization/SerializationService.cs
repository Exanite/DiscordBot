using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordBot.Core.Serialization
{
    public class SerializationService
    {
        private JsonSerializer serializer;

        private readonly IEnumerable<JsonConverter> converters;

        public SerializationService(IEnumerable<JsonConverter> converters)
        {
            this.converters = converters;
        }

        public JsonSerializer Serializer
        {
            get
            {
                return serializer ??= CreateJsonSerializer();
            }
        }

        private JsonSerializer CreateJsonSerializer()
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };

            foreach (var converter in converters)
            {
                settings.Converters.Add(converter);
            }

            return JsonSerializer.Create(settings);
        }
    }
}
