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
    [Route("/api/homework")]
    public class HomeworkController : ControllerBase
    {
        private readonly ICrudService<Homework> _homeworkService;
        private readonly ILogger<HomeworkController> _logger;
        
        public HomeworkController(ICrudService<Homework> homeworkService, ILogger<HomeworkController> logger)
        {
            _homeworkService = homeworkService;
            _logger = logger;
        }
        
        [HttpGet("{id}")]
        public ActionResult<Homework> GetHomework(int id)
        {
            return _homeworkService.Get(id) switch
            {
                null => NotFound(),
                var homework => homework
            };
        }
        
        [HttpGet]
        public ActionResult<IReadOnlyCollection<Homework>> GetHomeworks()
        {
            return _homeworkService.GetAll().ToArray();
        }
    
        [HttpPost]
        public IActionResult AddHomework(Homework homework)
        {
            try
            {
                var newHomeworkId = _homeworkService.New(homework);
                return Ok($"api/homework/{newHomeworkId}");
            }
            catch (Exception e)
            {
                _logger.LogError("Add Homework: There is already exist Homework with that id");
                throw new AlreadyExistenceException("There is already exist Homework with that id");
            }
        }
    
        [HttpPut("{id}")]
        public ActionResult<string> UpdateHomework(int id, Homework homework)
        {
            try
            {
                var homeworkId = _homeworkService.Edit(homework with { Id = id });
                return Ok($"api/homework/{homeworkId}");
            }
            catch (InvalidUserInputException e)
            {
                _logger.LogError("Edit Homework: There is no homework with that id");
                throw;
            }
        }
    
        [HttpDelete("{id}")]
        public ActionResult DeleteHomework(int id)
        {
            try
            {
                _homeworkService.Delete(id);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Delete Homework: There is no homework with that id");
                throw new InvalidUserInputException("There is no homework with that id");
            }
        }
    }
}