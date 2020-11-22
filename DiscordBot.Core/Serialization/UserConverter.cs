using System;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace DiscordBot.Core.Serialization
{
    public class UserConverter : JsonConverter<IUser>
    {
        private readonly DiscordSocketClient client;

        public UserConverter(DiscordSocketClient client)
        {
            this.client = client;
        }

        public override IUser ReadJson(JsonReader reader, Type objectType, IUser existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            ulong id = serializer.Deserialize<ulong>(reader);

            return client.GetUser(id);
        }

        public override void WriteJson(JsonWriter writer, IUser value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Id);
        }
    }
}
