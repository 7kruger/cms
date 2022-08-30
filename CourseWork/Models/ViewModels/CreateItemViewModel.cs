using System.ComponentModel.DataAnnotations;

namespace CourseWork.Models.ViewModels
{
    public class CreateItemViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string CollectionId { get; set; }
    }
}
