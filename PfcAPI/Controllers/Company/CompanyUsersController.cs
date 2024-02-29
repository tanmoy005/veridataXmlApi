using Microsoft.AspNetCore.Mvc;
using PfcAPI.Infrastucture.DBContext;
using PfcAPI.Infrastucture.Interfaces;

namespace PfcAPI.Controllers.Company
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyUsersController : ControllerBase
    {
        private readonly DbContextDB _context;

        private readonly IuserContext _userContext;
        public CompanyUsersController(DbContextDB context, IuserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        // GET: api/CompanyUsers
        //[HttpGet]
        //[Route("GetCompanyUserList")]
        //public ActionResult<BaseResponse<List<UserDetails>>> GetCompanyUsers()
        //{
        //    //if (_context.CompanyUsers == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //return await _context.CompanyUsers.ToListAsync();
        //    var UserList = Task.Run(async () => await _userContext.getUserListAsync(false)).GetAwaiter().GetResult();

        //    //if (UserList == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    return Ok(new BaseResponse<List<UserDetails>>(UserList));
        //}

        //// GET: api/CompanyUsers/5
        //[HttpGet]
        //[Route("GetCompanyUserDetails")]
        //public ActionResult<BaseResponse<UserDetails>> GetCompanyUsers(int? id)
        //{
        //    var _ErrorResponse = new ErrorResponse();
        //    if (id == null)
        //    {
        //        throw new ArgumentNullException("id");
        //    }
        //    var uid = id ?? 0;
        //    var CompanyUsers = Task.Run(async () => await _userContext.getUserDetailsAsync(uid, false)).GetAwaiter().GetResult();
        //    if (CompanyUsers == null)
        //    {
        //        _ErrorResponse.ErrorCode = 400;
        //        _ErrorResponse.UserMessage = "No User Found";
        //        _ErrorResponse.InternalMessage = "No User Found";

        //        return Ok(new BaseResponse<ErrorResponse>((HttpStatusCode.BadRequest, _ErrorResponse));
        //    }

        //    return Ok(new BaseResponse<UserDetails>(CompanyUsers));
        //}

        ////// PUT: api/CompanyUsers/5
        ////// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ////[HttpPut("{id}")]
        ////public async Task<IActionResult> PutCompanyUsers(int id, CompanyUsers companyUsers)
        ////{
        ////    if (id != companyUsers.Id)
        ////    {
        ////        return BadRequest();
        ////    }

        ////    _context.Entry(companyUsers).State = EntityState.Modified;

        ////    try
        ////    {
        ////        await _context.SaveChangesAsync();
        ////    }
        ////    catch (DbUpdateConcurrencyException)
        ////    {
        ////        if (!CompanyUsersExists(id))
        ////        {
        ////            return NotFound();
        ////        }
        ////        else
        ////        {
        ////            throw;
        ////        }
        ////    }

        ////    return NoContent();
        ////}

        ////// POST: api/CompanyUsers
        ////// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ////[HttpPost]
        ////public async Task<ActionResult<CompanyUsers>> PostCompanyUsers(CompanyUsers companyUsers)
        ////{
        ////    if (_context.CompanyUsers == null)
        ////    {
        ////        return Problem("Entity set 'DbContextDB.CompanyUsers'  is null.");
        ////    }
        ////    _context.CompanyUsers.Add(companyUsers);
        ////    await _context.SaveChangesAsync();

        ////    return CreatedAtAction("GetCompanyUsers", new { id = companyUsers.Id }, companyUsers);
        ////}

        //// DELETE: api/CompanyUsers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCompanyUsers(int id)
        //{
        //    //if (_context.AppUsers == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //var ValidateUser = Task.Run(async () => await _userContext.validateUserById(id, false)).GetAwaiter().GetResult();
        //    //var applicationUsers = await _context.AppUsers.FindAsync(id);
        //    //if (ValidateUser == false)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //var RemoveUser = Task.Run(async () => await _userContext.RemoveUserDetails(id, false)).GetAwaiter().GetResult();
        //    //if (!RemoveUser)
        //    //{
        //    //    return NotFound();
        //    //}
        //    return NoContent();
        //}

        ////private bool CompanyUsersExists(int id)
        ////{
        ////    return (_context.CompanyUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        ////}
    }
}
