using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApi.Serializers;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("/api/attendance_report")]
    public class AttendanceReportController : ControllerBase
    {
        private readonly ILectionLogService _lectionLogService;
        private readonly ICrudService<Student> _studentService;
        private readonly ICrudService<Lection> _lectionService;
        private readonly IAttendanceReport _reportService;
        
        private readonly ILogger<AttendanceReportController> _logger;
        
        public AttendanceReportController(
                ILectionLogService lectionLogService, 
                ICrudService<Lection> lectionService, 
                ICrudService<Student> studentService, 
                IAttendanceReport reportService,
                ILogger<AttendanceReportController> logger)
        {
            _logger = logger;
            _lectionService = lectionService;
            _lectionLogService = lectionLogService;
            _studentService = studentService;
            _reportService = reportService;
        }
        
        
        [HttpPost("create-report")]
        public IActionResult CreateReport(bool lectionAttendance, bool studentAttendance, string lectionName, string studentName)
        {
            var jsonSerializator = new JsonSerializator();
            var xmlSerializator = new XmlSerializator();

            if (lectionAttendance)
            {
                _reportService.GenerateAttendanceLectionReport(lectionName, jsonSerializator);
                _reportService.GenerateAttendanceLectionReport(lectionName, xmlSerializator);
            }
            if (studentAttendance)
            {
                _reportService.GenerateAttendanceStudentReport(studentName, jsonSerializator);
                _reportService.GenerateAttendanceStudentReport(studentName, xmlSerializator);
            }

            return Ok();
        }
    }
}