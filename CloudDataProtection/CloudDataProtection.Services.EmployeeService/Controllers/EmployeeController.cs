using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CloudDataProtection.Core.Controllers;
using CloudDataProtection.Core.Controllers.Dto.Output;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Rest.Errors;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.EmployeeService.Business;
using CloudDataProtection.Services.EmployeeService.Controllers.Dto.Input;
using CloudDataProtection.Services.EmployeeService.Controllers.Dto.Output;
using CloudDataProtection.Services.EmployeeService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Services.EmployeeService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class EmployeeController : ServiceController
    {
        private readonly EmployeeBusinessLogic _logic;
        private readonly IMapper _mapper;

        public EmployeeController(EmployeeBusinessLogic logic, IMapper mapper, IJwtDecoder jwtDecoder) : base(jwtDecoder)
        {
            _logic = logic;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult> Index([FromQuery] GetAllEmployeesInput input)
        {
            PaginatedBusinessResult<Employee> result = await _logic.Search(input.Skip, input.PageSize, input.OrderBy.Trim(), input.SearchQuery);

            if (!result.Success)
            {
                return Problem(result.Message);
            }

            return new OkObjectResult(new PaginatedOutput<EmployeeOutput>
            {
                Data = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeOutput>>(result.Data),
                ItemCount = result.ItemCount
            });
        }
        
        [HttpPost("")]
        public async Task<ActionResult> Create([FromBody] CreateEmployeeInput input)
        {
            Employee employee = _mapper.Map<CreateEmployeeInput, Employee>(input);
            
            employee.SetAuditingInfo(UserId);
            
            BusinessResult<Employee> result = await _logic.Create(employee);

            if (!result.Success)
            {
                switch (result.ErrorType)
                {
                    case ResultError.Conflict:
                        return Problem(result.Message, null, (int) HttpStatusCode.Conflict, "Conflict");
                    default:
                        return Problem(result.Message);
                }
            }

            return new OkObjectResult(_mapper.Map<Employee, EmployeeOutput>(result.Data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            BusinessResult<Employee> result = await _logic.Get(id);

            if (!result.Success)
            {
                return NotFound(NotFoundResponse.Create("Employee", id));
            }

            return Ok(_mapper.Map<EmployeeOutput>(result.Data));
        }
    }
}