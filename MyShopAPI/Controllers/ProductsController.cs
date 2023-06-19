using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShopAPI.Contexts;
using MyShopAPI.Models;

namespace MyShopAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase {
        private readonly MyShopContext _context;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public ProductsController(IConfiguration config, ILogger<ProductsController> logger, MyShopContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAll() {
            try {
                List<Product> items = await _context.Products.ToListAsync();
                return Ok(items);
            } catch (Exception ex) {
                _logger.LogCritical("ERROR in [ProductsController.GetAll], " + ex.Message, ex);
                return StatusCode(500, "Sorry, we ran into an issue, our dev has been notified, try again later!!!");
            }
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> Get(int id) {
            try {
                Product item = await _context.Products.FindAsync(id);
                if (item == null) {
                    return NotFound();
                }
                return Ok(item);
            } catch (Exception ex) {
                _logger.LogCritical("ERROR in [ProductsController.Get], " + ex.Message, ex);
                return StatusCode(500, "Sorry, we ran into an issue, our dev has been notified, try again later!!!");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Insert(Product item) {
            try {
                await _context.Products.AddAsync(item);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                                    "Get",
                                    new { id = item.ID },
                                    item
                                    );
            } catch (Exception ex) {
                _logger.LogCritical("ERROR in [ProductsController.Insert], " + ex.Message, ex);
                return StatusCode(500, "Sorry, we ran into an issue, our dev has been notified, try again later!!!");
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Update(Product item) {
            try {
                if (!_context.Products.Any(p => p.ID == item.ID)) {
                    return NotFound();
                }

                _context.Entry(item).State = EntityState.Modified;


                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException ex) {
                if (!_context.Products.Any(p => p.ID == item.ID)) {
                    return NotFound();
                } else {
                    throw;
                }
            } catch (Exception ex) {
                _logger.LogCritical("ERROR in [ProductsController.Update], " + ex.Message, ex);
                return StatusCode(500, "Sorry, we ran into an issue, our dev has been notified, try again later!!!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id) {
            Product item = await _context.Products.FindAsync(id);

            if (item == null) {
                return NotFound();
            }

            _context.Products.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}