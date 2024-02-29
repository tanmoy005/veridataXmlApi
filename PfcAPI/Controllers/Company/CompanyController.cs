using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PfcAPI.Infrastucture.DBContext;
using PfcAPI.Model.Company;

namespace PfcAPI.Controllers.Company
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly DbContextDB _context;

        public CompanyController(DbContextDB context)
        {
            _context = context;
        }

        // GET: api/Company
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyInfo>>> GetCompanyDetails()
        {
            if (_context.CompanyDetails == null)
            {
                return NotFound();
            }
            return await _context.CompanyDetails.ToListAsync();
        }

        // GET: api/Company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyInfo>> GetCompanyInfo(int id)
        {
            if (_context.CompanyDetails == null)
            {
                return NotFound();
            }
            var companyInfo = await _context.CompanyDetails.FindAsync(id);

            if (companyInfo == null)
            {
                return NotFound();
            }

            return companyInfo;
        }

        // PUT: api/Company/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompanyInfo(int id, CompanyInfo companyInfo)
        {
            if (id != companyInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(companyInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyInfoExists(id))
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

        // POST: api/Company
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CompanyInfo>> PostCompanyInfo(CompanyInfo companyInfo)
        {
            if (_context.CompanyDetails == null)
            {
                return Problem("Entity set 'DbContextDB.CompanyDetails'  is null.");
            }
            _context.CompanyDetails.Add(companyInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompanyInfo", new { id = companyInfo.Id }, companyInfo);
        }

        // DELETE: api/Company/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyInfo(int id)
        {
            if (_context.CompanyDetails == null)
            {
                return NotFound();
            }
            var companyInfo = await _context.CompanyDetails.FindAsync(id);
            if (companyInfo == null)
            {
                return NotFound();
            }

            _context.CompanyDetails.Remove(companyInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyInfoExists(int id)
        {
            return (_context.CompanyDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
