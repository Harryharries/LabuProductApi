using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversion;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
            {
                return NotFound("No products found");
            }
            var (_, list) = ProductConversion.FromEntity(null!, products);
            return list!.Any() ? Ok(list) : NotFound("No products found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await productInterface.FindByIdAsync(id);
            if (product is null)
            {
                return NotFound("Product not found");
            }
            var (singleProduct, _) = ProductConversion.FromEntity(product, null!);
            return singleProduct is not null ? Ok(singleProduct) : NotFound("Product not found");
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newProduct = await productInterface.CreateAsync(ProductConversion.ToEntity(product));
            if (newProduct is null)
            {
                return NotFound("Product not found");
            }
            return newProduct is not null ? Ok(newProduct) : BadRequest(newProduct);
        }

        [HttpPut]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedProduct = await productInterface.UpdateAsync(ProductConversion.ToEntity(product));
            if (updatedProduct is null)
            {
                return NotFound("Product not found");
            }
            return updatedProduct is not null ? Ok(updatedProduct) : BadRequest(updatedProduct);
        }

        [HttpDelete]
        public async Task<ActionResult<ProductDTO>> DeleteProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var deletedProduct = await productInterface.DeleteAsync(ProductConversion.ToEntity(product));
            if (deletedProduct is null)
            {
                return NotFound("Product not found");
            }
            return deletedProduct is not null ? Ok(deletedProduct) : BadRequest(deletedProduct);
        }   
    }
}
