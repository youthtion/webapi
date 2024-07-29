using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApi.Models;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;

        public OrdersController(OrderContext context)
        {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderData>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<OrderData>> PostOrder(OrderData _order)
        {
            if(!_order.checkInput()){
                return BadRequest("Input invalid");
            }
            if(checkIdExists(_order.Id)){
                return BadRequest("Id duplicate");
            }

            OrderManager order_manager = new OrderManager(_order, "English");            
            order_manager.checkName();
            order_manager.exchangeCurrency("TWD");
            order_manager.checkPriceLimit();
            
            string error_txt = order_manager.getErrorText();
            if(error_txt.Length > 0){
                return BadRequest(error_txt);
            }

            _context.Orders.Add(order_manager.getOrderData());
            await _context.SaveChangesAsync();

            return order_manager.getOrderData();
        }

        private bool checkIdExists(string _id)
        {
            return _context.Orders.Any(e => e.Id == _id);
        }
    }
}
