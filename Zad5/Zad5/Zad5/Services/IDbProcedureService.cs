using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zad5.Model
{
    public interface IDbProcedureService
    {
        public bool addWarehouse(WarehouseRequest warehouseRequest);
    }
}
