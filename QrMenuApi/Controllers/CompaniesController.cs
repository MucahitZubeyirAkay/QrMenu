using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrMenuApi.Data.Context;
using QrMenuApi.Data.Models;

namespace QrMenuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly QrMenuApiContext _context;

        public CompaniesController(QrMenuApiContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        [Authorize]
        public ActionResult<List<Company>> GetCompanies()
        {
          if (_context.Companies == null)
          {
              return NotFound();
          }
            return _context.Companies.ToList();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public ActionResult<Company> GetCompany(int id)
        {
          
            var company =  _context.Companies?.Find(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Companies
        [HttpPost]
        public ActionResult<Company> PostCompany(Company company)
        {
            company.ParentCompany = null;

            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
       
            var company =  _context.Companies?.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(int id)
        {
            return (_context.Companies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
