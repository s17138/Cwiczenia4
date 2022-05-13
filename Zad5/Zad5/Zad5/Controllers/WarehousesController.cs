using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zad5.Model;

namespace Zad5.Controllers
{
    [Route("api/warehouses")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseDbRepository _warehouseDbRepository;

        public WarehousesController(IWarehouseDbRepository warehouseDbRepository)
        {
            _warehouseDbRepository = warehouseDbRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddWarehouse(WarehouseRequest warehouseRequest)
        {
            if (warehouseRequest.CreatedAt == null
               || warehouseRequest.Amount == null
               || warehouseRequest.Amount <= 0
               || warehouseRequest.IdProduct == null
               || warehouseRequest.IdWarehouse == null)
            {
                return BadRequest();
            }
            int result = await _warehouseDbRepository.addWarehouse(warehouseRequest);
            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                switch(result)
                {
                    case -1:
                        return NotFound("Order not found");
                    case -2:
                        return BadRequest("Order has been realized");
                    case -3:
                        return NotFound("Warehouse or product does not exist");
                    default:
                        return BadRequest();
                }
            }
        }
    }
}
