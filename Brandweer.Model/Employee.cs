using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandweer.Model
{
    public class Employee
    {
        public int Id { get; set; } 

        public required string FirstName { get; set; }   

        public required string LastName { get; set; }   
    }
}
