using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json.Serialization;

namespace CourseWork.Domain.Models
{
    public class CommentModel
    {
        [JsonPropertyName("id")]
        [ModelBinder(Name = "id")]
        public long Id { get; set; }
        public string SrcId { get; set; }
        [JsonPropertyName("parent")]
        [ModelBinder(Name = "parent")]
        public long? Parent { get; set; }
        [JsonPropertyName("created")]
        [ModelBinder(Name = "created")]
        public DateTime Created { get; set; }
        [JsonPropertyName("modified")]
        [ModelBinder(Name = "modified")]
        public DateTime Modified { get; set; }
        [JsonPropertyName("content")]
        [ModelBinder(Name = "content")]
        public string Content { get; set; }
        [JsonPropertyName("creator")]
        [ModelBinder(Name = "creator")]
        public int Creator { get; set; }
        [JsonPropertyName("fullname")]
        [ModelBinder(Name = "fullname")]
        public string Fullname { get; set; }
        [JsonPropertyName("profile_pricture_url")]
        [ModelBinder(Name = "profile_pricture_url")]
        public string? ProfilePictureUrl { get; set; }
        [JsonPropertyName("upvote_count")]
        [ModelBinder(Name = "upvote_count")]
        public int UpvoteCount { get; set; }
        [JsonPropertyName("user_has_upvoted")]
        [ModelBinder(Name = "user_has_upvoted")]
        public bool UserHasUpvoted { get; set; }
        [JsonPropertyName("created_by_admin")]
        [ModelBinder(Name = "created_by_admin")]
        public bool CreatedByAdmin { get; set; }
        [JsonPropertyName("created_by_current_user")]
        [ModelBinder(Name = "created_by_current_user")]
        public bool CreatedByCurrentUser { get; set; }
    }
}
