using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.WebApi.Data.Entities
{
    public class StudentEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, Range(1, int.MaxValue)]
        public int No { get; set; }
        [Required, StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Required, StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; } = string.Empty;
        [Required, StringLength(20, MinimumLength = 3)]
        public string Class {  get; set; } = string.Empty;

    }
}
