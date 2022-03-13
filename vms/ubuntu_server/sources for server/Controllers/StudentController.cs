using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Helpers;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("/api/student")]
    public class StudentController : ControllerBase
    {
        private readonly ICrudService<Student> _studentService;
        private readonly ILogger<StudentController> _logger;
        
        public StudentController(ICrudService<Student> studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }
        
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            return _studentService.Get(id) switch
            {
                null => NotFound(),
                var student => student
            };
        }
        
        [HttpGet]
        public ActionResult<IReadOnlyCollection<Student>> GetStudents()
        {
            return _studentService.GetAll().ToArray();
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            try
            {
                var newPersonId = _studentService.New(student);
                return Ok($"api/student/{newPersonId}");
            }
            catch (Exception e)
            {
                _logger.LogError("Add Student: There is already exist Student with that id");
                throw new AlreadyExistenceException("There is already exist Student with that id");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<string> UpdateStudent(int id, Student student)
        {
            try
            {
                var studentId = _studentService.Edit(student with { Id = id });
                return Ok($"api/student/{studentId}");
            }
            catch (InvalidUserInputException e)
            {
                _logger.LogError("Edit Student: There is no Student with that id");
                throw;
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(int id)
        {
            try
            {
                _studentService.Delete(id);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Delete Student: There is no Student with that id");
                throw new InvalidUserInputException("There is no Student with that id");
            }
        }
    }
}