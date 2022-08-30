using System.ComponentModel.DataAnnotations;
using CourseWork.Models.Enums;

namespace CourseWork.Models.ViewModels
{
    public class CreateCollectionViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Theme Theme { get; set; }

    }
}
