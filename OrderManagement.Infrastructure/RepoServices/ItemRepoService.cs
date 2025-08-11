using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Domain.Entities;
using OrderManagement.Core.Domain.RepoContracts;
using OrderManagement.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.RepoServices
{
    public class ItemRepoService(AppDbContext context) : IItemRepoContract
    {
        private readonly AppDbContext _db = context;

        public async Task<Item> AddItemAsync(Item item)
        {
            await  _db.Items.AddAsync(item);
            return await _db.SaveChangesAsync().ContinueWith(t => item);
        }

        public async Task<bool> DeleteItemAsync(Guid itemId)
        {
            _db.Items.RemoveRange(_db.Items.Where(i => i.ItemId == itemId));
            int row = await _db.SaveChangesAsync();
            return row > 0; // Deletion successful

        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _db.Items.Include(i=>i.OrderItems).ToListAsync();

        }

        public async Task<Item?> GetItemByIdAsync(Guid itemId)
        {
           return await _db.Items.Include(p => p.OrderItems)
                                    .FirstOrDefaultAsync(p => p.ItemId == itemId);
        }

        public async Task<Item> UpdateItemAsync(Item item)
        {
            Item? itemMatching = await _db.Items.FirstOrDefaultAsync(i => i.ItemId == item.ItemId);
            if (itemMatching == null)
            {
                return item;
            }
            itemMatching.ItemName = item.ItemName;
            itemMatching.ItemPrice = item.ItemPrice;
            itemMatching.ItemTotalQuantity = item.ItemTotalQuantity;
            itemMatching.OrderItems = item.OrderItems;
            await _db.SaveChangesAsync();
            return itemMatching;
        }
    }
    }
