﻿using Asp.Versioning;
using Dotnet8WepApiJwt.Data;
using Dotnet8WepApiJwt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotnet8WepApiJwt.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public PagesController(ILogger<PagesController> logger, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("new")]
        public async Task<ActionResult<Page>> CreatePage(PageDto pageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var page = new Page
            {
                Id = pageDto.Id,
                Title = pageDto.Title,
                Author = pageDto.Author,
                Body = pageDto.Body,
            };

            _dbContext.Pages.Add(page);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPage), new { id = page.Id }, page);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<PageDto>> GetPage(int id)
        {
            var page = await _dbContext.Pages.FindAsync(id);

            if (page is null)
            {
                return NotFound();
            }

            var pageDto = new PageDto
            {
                Id = page.Id,
                Author = page.Author,
                Body = page.Body,
                Title = page.Title
            };

            return pageDto;
        }


        [HttpGet]
        public async Task<PagesDto> ListPages()
        {
            var pagesFromDb = await _dbContext.Pages.ToListAsync();

            var pagesDto = new PagesDto();

            foreach (var page in pagesFromDb)
            {
                var pageDto = new PageDto
                {
                    Id = page.Id,
                    Author = page.Author,
                    Body = page.Body,
                    Title = page.Title
                };

                pagesDto.Pages.Add(pageDto);
            }

            return pagesDto;
        }
    }
}
