using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using static Play.Catalog.Service.Dtos;
using System;
using System.Linq;
using Play.Catalog.Service.Repositories;
using System.Threading.Tasks;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemRepository itemsRepository = new();

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync()).Select(items => items.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetItemAsync(id);
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreatedItemDto createdItemDto)
        {
            var item = new Item
            {
                Name = createdItemDto.Name,
                Description = createdItemDto.Description,
                Price = createdItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new
            {
                id = item.Id
            }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdatedItemDto updatedItemDto)
        {
            var existingItem = await itemsRepository.GetItemAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = updatedItemDto.Name;
            existingItem.Description = updatedItemDto.Description;
            existingItem.Price = updatedItemDto.Price;
            await itemsRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await itemsRepository.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await itemsRepository.RemoveAsync(item.Id);

            return NoContent();
        }
    }
}