using System.ComponentModel.DataAnnotations;
using CourseWork.Models.Enums;
using CourseWork.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CourseWork.Models.ViewModels
{
    public class EditCollectionViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Нет названия коллекции")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Нет указана тема коллекции")]
        public Theme Theme { get; set; }
        public string ImgRef { get; set; }
        public List<Item> Items { get; set; }
    }
}
