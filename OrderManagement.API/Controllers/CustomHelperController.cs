using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OrderManagement.API.Controllers
{
    /// <summary>
    /// ControllerBasefor custom helper methods that can be used across multiple controllers.
    /// </summary>
    [ApiController] 
    [Route("api/v{version:apiVersion}/[controller]")]

    public class CustomHelperController : ControllerBase
    {
    }
}
