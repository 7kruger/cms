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
        [HiddenInput(DisplayValue = false)]
        public string Author { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public Theme Theme { get; set; }
        public List<Item> Items { get; set; }
    }
}
