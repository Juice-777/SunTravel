using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTravel.Models
{
    interface ICountry
    {
        int Id { get; set; }
        string Name { get; set; }
        byte[] Photo1 { get; set; }
        string Description { get; set; }
    }
}
