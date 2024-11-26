using Microsoft.AspNetCore.Mvc;

namespace NZWalksAPI.Controllers {
    [ApiController]
    // https://localhost:portNumber/api/students
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase {
        // GET: https://localhost:portNumber/api/students
        [HttpGet]
        public IActionResult GetAllStudents() {
            string[] studentNames = ["John", "Jane", "Jim", "Jill"];
            return Ok(studentNames);
        }
    }
}
