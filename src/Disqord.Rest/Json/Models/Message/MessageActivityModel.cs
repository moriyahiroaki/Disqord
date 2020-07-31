﻿using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class MessageActivityModel : JsonModel
    {
        [JsonProperty("type")]
        public MessageActivityType Type { get; set; }

        [JsonProperty("party_id")]
        public Optional<string> PartyId { get; set; }
    }
}
