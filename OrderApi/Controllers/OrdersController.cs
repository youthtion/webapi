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
            // 空值檢查
            if(!_order.checkInput()){
                return BadRequest("Input invalid");
            }
            // id重複檢查
            if(checkIdExists(_order.Id)){
                return BadRequest("Id duplicate");
            }

            // 處理剩下的邏輯檢查
            OrderManager order_manager = new OrderManager(_order, "English");            
            order_manager.checkName(); // 檢查名字格式
            order_manager.exchangeCurrency("TWD"); // 匯率轉換為TWD
            order_manager.checkPriceLimit(); // 檢查金額上限
            
            // 檢查失敗回傳
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
