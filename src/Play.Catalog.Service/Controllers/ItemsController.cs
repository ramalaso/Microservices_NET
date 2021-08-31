using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using static Play.Catalog.Service.Dtos;
using System;
using System.Linq;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new List<ItemDto>(){
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amunt of HP", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow),
        };

        [HttpGet]
        public IEnumerable<ItemDto> GetItemDtos()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ItemDto GetById(Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            return item;
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreatedItemDto createdItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createdItemDto.Name, createdItemDto.Description, createdItemDto.Price, DateTimeOffset.Now);
            items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdatedItemDto updatedItemDto)
        {
            var existingItem = items.Where(item => item.Id == id).SingleOrDefault();

            var updatedItem = existingItem with
            {
                Name = updatedItemDto.Name,
                Description = updatedItemDto.Description,
                Price = updatedItemDto.Price
            };

            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items[index] = updatedItem;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            if (index == -1)
                return NotFound();
            items.RemoveAt(index);
            return NoContent();
        }
    }
}