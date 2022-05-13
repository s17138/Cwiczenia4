using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zad5.Model
{
    public class WarehouseRequest
    {
        public int? IdProduct { get; set; }
        public int? IdWarehouse { get; set; }
        public int? Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
