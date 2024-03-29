﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrMenuApi.Data.Context;
using QrMenuApi.Data.DtoModels;
using QrMenuApi.Data.Models;
using QrMenuApi.Services;

namespace QrMenuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly QrMenuApiContext _context;

        public CompaniesController(IMapper mapper, QrMenuApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        public ActionResult<List<Company>> GetCompanies()
        {
            if(_context.Companies == null)
            {
                return NotFound();
            }

          List<Company> company= _context.Companies!.ToList();

          return company;
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public ActionResult<Company> GetCompany(int id)
        {
            Company? company = _context.Companies!.Find(id);
            if(company == null)
            {
                return NotFound();
            }
            return company;
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        public ActionResult PutCompany(int id, CompanyDto companyDto)
        {
            var existingCompany = _context.Companies!.Find(id);

            if (existingCompany == null)
            {
                return NotFound();
            }

            // AutoMapper kullanarak DTO'yu model nesnesine eşleme
            var company = _mapper.Map(companyDto, existingCompany);

            //eşlenen modeli veritabanı ile güncelleme
            _context.Entry(company).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
                return Ok("Güncelleme başarılı");
            }
            catch
            {
                return NoContent();
            }
            
        }

        // POST: api/Companies
        [HttpPost]
        public ActionResult<Company> PostCompany(CompanyDto companyDto)
        {
            if(companyDto == null)
            {
                return NotFound(); 
            }

            var company = _mapper.Map<Company>(companyDto);
            _context.Companies!.Add(company);
            _context.SaveChanges();
            return Ok();
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public ActionResult DeleteCompany(int id)
        {
            var company = _context.Companies!.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            company.StateId = 0;
            _context.Companies!.Update(company);


            try
            {

                _context.Companies!.Update(company);
                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }

        private bool CompanyExists(int id)
        {
            return (_context.Companies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
