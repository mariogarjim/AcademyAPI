using API.DTO;
using API.Interfaces;
using API.Services;
using API.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using API.Settings;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ITableStorageService _storageService;
        private readonly IOptions<TenantSettings> _tenantSettings;
        private readonly IHttpContextAccessor _contextAccessor;
        


        public StudentsController(ITableStorageService storageService, IHttpContextAccessor contextAccessor, 
                                 ITenantSettingsFactory tenantSettingsFactory)
        {
            _tenantSettings = tenantSettingsFactory.GetTenantSettings();
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _contextAccessor = contextAccessor;
        }

        [HttpGet("Tenants")]
        [ActionName(nameof(GetAllAsync))]
        public IActionResult GetAllTenants()
        {
            List<Tenant> tenants = _tenantSettings.Value.Tenants;
            return Ok(tenants);
        }

        [HttpGet]
        [ActionName(nameof(GetAllAsync))]
        public async Task<IActionResult> GetAllAsync()
        {  
            TenantService ts = new TenantService(this._tenantSettings, _contextAccessor);
            var alumns = await _storageService.GetAllEntityAsync(ts.GetTenant().TID);
            if (!alumns.Any()) return NotFound();
            return Ok(alumns);

        }
        

        [HttpGet(@"id/{id:regex(" + RegexController.regexID + ")}")]
        [ActionName(nameof(GetAsyncById))]
        public async Task<IActionResult> GetAsyncById(string id)
        {
            TenantService ts = new TenantService(this._tenantSettings, _contextAccessor);
            var alumns = await _storageService.GetEntityAsyncById(ts.GetTenant().TID , id);
            if (alumns is null) return NotFound();
            return Ok(alumns);
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateAlumnDto alumn)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (alumn.DateOfBirth >= DateTime.UtcNow)
                return BadRequest("Date of birth is invalid");

            TenantService ts = new TenantService(this._tenantSettings, _contextAccessor);

            var existentStudent = await _storageService.GetEntityAsyncById(ts.GetTenant().TID, alumn.ID);
            if (existentStudent is not null)
                return BadRequest("Student already exists");
            
            var createEntity = await _storageService.UpsertEntityAsync(alumn, ts.GetTenant().TID);
            
            return CreatedAtAction(nameof(GetAsyncById), new { tenant = createEntity.University, id = createEntity.ID }, createEntity);
        }

        [HttpDelete(@"id/{id:regex(" + RegexController.regexID + ")}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            TenantService ts = new TenantService(this._tenantSettings, _contextAccessor);

            var existentStudent = await _storageService.GetEntityAsyncById(ts.GetTenant().TID, id);
            if(existentStudent is null) return BadRequest("Student doesn't exist");
            await _storageService.DeleteEntityAsync(ts.GetTenant().TID, id);
            return NoContent();
        }

        /* Updates an Alumn in the DataBase in case it respects the following criteria:
         * 1. Alumn with the same country and id exists,
         * 2. The fields have to be valid.
         * Returns an AlumnDTO of the updated Alumn.
         */
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] CreateAlumnDto alumn)
        {
            TenantService ts = new TenantService(this._tenantSettings, _contextAccessor);
            // Check if fields are valid.
            if (!ModelState.IsValid)
                return BadRequest();
            if (alumn.DateOfBirth >= DateTime.UtcNow)
                return BadRequest("Date of birth is invalid");

            // Check if Alumn exists in Database.
            var existentStudent = await _storageService.GetEntityAsyncById(ts.GetTenant().TID, alumn.ID);
            if (existentStudent is null) return BadRequest("Student doesn't exist");

            // Update Alumn in Database
            var updatedEntity = await _storageService.UpsertEntityAsync(alumn, ts.GetTenant().TID);

            return Ok(updatedEntity);
        }


   }

        
    
}
