using System.Security.Claims;
using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Category>>> GetAllCategoriesAsync()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        return Ok(category);
    }
    
    [HttpGet("parents")]
    public async Task<ActionResult<ICollection<Category>>> GetParentCategoriesAsync()
    {
        var categories = _categoryService.GetAllParentCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id}/children")]
    public async Task<ActionResult<Category>> GetCategoryChildrenAsync(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        return Ok(category);
    }

    [HttpGet("{id}/products")]
    public async Task<ActionResult<ICollection<Product>>> GetProductsByCategoryIdAsync(int id)
    {
        var products = await _categoryService.GetProductsByCategoryIdAsync(id);
        return Ok(products);
    }
}