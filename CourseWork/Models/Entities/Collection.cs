using System.Collections.Generic;
using CourseWork.Models.Enums;
using System;

namespace CourseWork.Models.Entities
{
    public class Collection
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public Theme Theme { get; set; }
        public string ImgRef { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public Collection()
        {
            Items = new List<Item>();
        }
    }
}
