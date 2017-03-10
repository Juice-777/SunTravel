using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTravel.Models
{
    interface IService
    {
        bool Insurance { get; set; } //Страховка
        Food Food { get; set; }
    }
}
