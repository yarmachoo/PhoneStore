﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Yarmak.API.Data;
using WEB_253503_Yarmak.API.Services;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;

namespace WEB_253503_Yarmak.API.Controllers
{
    [Route("phone")]
    [ApiController]
    public class PhonesController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly AppDbContext _context;

        public PhonesController(AppDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: phone/phones/[category]
        [HttpGet("phones/{category?}")]
        public async Task<ActionResult<ResponseData<List<Phone>>>> GetPhones(string? category=null)
        {
            return Ok(await _productService.GetProductListWithoutPageAsync(category));
        }

        // GET: phone/getphones/[category]?pageNo=1
        [HttpGet("getphones/{category?}")]
        public async Task<ActionResult<ResponseData<List<Phone>>>> GetPhones(
            string? category = null,
            int pageNo = 1,
            int pageSize = 3)
        {
            return Ok(await _productService.GetProductListAsync(category,
                pageNo,
                pageSize));
        }

        // GET: phone/getphone/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<Phone>>> GetPhone(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            if (result.Data == null || !result.Successfull)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result);
        }

        // PUT: api/Phones/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutPhone(int id, Phone phone)
        {
            if (id != phone.Id)
            {
                return BadRequest();
            }

            await _productService.UpdateProductAsync(id, phone);

            return NoContent();
        }

        // POST: api/Phones
        [HttpPost]
        public async Task<ActionResult<ResponseData<Phone>>> PostPhone(Phone phone)
        {
            var result = await _productService.CreateProductAsync(phone);

            return CreatedAtAction("GetPhone", new { id = result?.Data?.Id }, result);
        }

        // DELETE: phone/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePhone(int id)
        {
            await _productService.DeleteproductAsync(id);
            return NoContent();
        }
    }
}
