using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Owner Id")]
        public int OwnerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Breed { get; set; }
        public string Notes { get; set; }
        [DisplayName("Image")]
        public string ImageURL { get; set; }
    }
}
