using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTravel.Models
{
    interface ITour : IService
    {
        int Id { get; set; }
        string Name { get; set; }
        int Place { get; set; } //count place in room
        DateTime DateStart { get; set; }
        int Duration { get; set; } //продолжительность путёвки
        decimal Price { get; set; }
        int FreePlace { get; set; } //количество свободных путёвок
        byte[] Photo1 { get; set; }
        int Active { get; set; }
    }
}
