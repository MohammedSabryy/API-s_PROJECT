using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    
    public class BasketController(IServiceManager serviceManeger) : ApiController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDTO>> Get(string id)
        {
            var basket = await serviceManeger.BasketService.GetBasketAsync(id);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> Update(BasketDTO basketDTO)
        {
            var basket = await serviceManeger.BasketService.UpdateBasketAsync(basketDTO);
            return Ok(basket);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await serviceManeger.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
