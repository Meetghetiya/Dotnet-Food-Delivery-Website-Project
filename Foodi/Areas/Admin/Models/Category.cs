using System.ComponentModel.DataAnnotations;

namespace Foodi.Areas.Admin.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(255, ErrorMessage = "Category name must be less than 255 characters.")]
        public string? CategoryName { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Please specify whether this category is active or not.")]
        public bool IsActive { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }
    }

    
}
