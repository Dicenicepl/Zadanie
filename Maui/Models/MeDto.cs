using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Models
{
    public class MeDto
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; }= string.Empty;
        public string LastName { get; set; }= string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.MinValue;
    }
}
