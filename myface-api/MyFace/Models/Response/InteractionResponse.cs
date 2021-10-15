using System;
using System.Text.Json.Serialization;
using MyFace.Models.Database;

namespace MyFace.Models.Response
{
    public class InteractionResponse
    {
        private readonly Interaction _interaction;

        public InteractionResponse(Interaction interaction)
        {
            _interaction = interaction;
        }

        public InteractionResponse(Interaction interaction, Post post)
        {
            _interaction = interaction;
            Post = post;
        }

        public Post Post { get; set; }
        public int Id => _interaction.Id;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public InteractionType Type => _interaction.Type;
        public DateTime Date => _interaction.Date;
        public int PostId => _interaction.PostId;
        public int UserId => _interaction.UserId;
    }
}