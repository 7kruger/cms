using System.ComponentModel.DataAnnotations;

namespace CourseWork.Models.ViewModels
{
    public class CreateItemViewModel
    {
        [Required(ErrorMessage = "Нет названия айтема")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Нет содержания")]
        public string Content { get; set; }
        public string CollectionId { get; set; }
    }
}
