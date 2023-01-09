using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Models
{
    internal class Customer
    {
        public int Id { get; set; }

        public Person Person { get; set; }

        public int Balance { get; set; }

        public bool IsBanned { get; set; }
    }
}
