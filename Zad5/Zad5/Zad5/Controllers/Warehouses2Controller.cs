using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zad5.Model;

namespace Zad5.Controllers
{
    [Route("api/warehouses2")]
    [ApiController]
    public class Warehouses2Controller : ControllerBase
    {
        private readonly IDbProcedureService _warehouseProcedureRepository;

        public Warehouses2Controller(IDbProcedureService warehouseProcedureRepository)
        {
            _warehouseProcedureRepository = warehouseProcedureRepository;
        }

        [HttpPost]
        public IActionResult AddWarehouse(WarehouseRequest warehouseRequest)
        {
            if(warehouseRequest.CreatedAt == null 
                || warehouseRequest.Amount == null 
                || warehouseRequest.Amount <=0 
                || warehouseRequest.IdProduct == null 
                || warehouseRequest.IdWarehouse == null)
            {
                return BadRequest();
            }
            if (_warehouseProcedureRepository.addWarehouse(warehouseRequest))
            {
                return Ok();
            } else
            {
                return NotFound();
            }
        }
    }
}
