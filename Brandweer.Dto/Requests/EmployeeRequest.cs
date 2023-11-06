using System.ComponentModel.DataAnnotations;

namespace Brandweer.Dto.Requests
{
    public class EmployeeRequest
    {
        [Display(Name = "First Name")]
        [Required]
        public required string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public required string LastName { get; set; }

    }
}
