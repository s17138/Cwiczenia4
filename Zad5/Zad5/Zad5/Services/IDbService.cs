using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zad5.Model
{
    public interface IDbService
    {
        public Task<int?> addWarehouse(WarehouseRequest warehouseRequest);
    }
}
