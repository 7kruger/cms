using System;

namespace CourseWork.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string CollectionId { get; set; }
        public string ItemId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
