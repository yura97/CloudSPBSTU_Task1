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
    [Route("/api/lector_log")]
    public class LectionLogController : ControllerBase
    {
        private readonly ILectionLogService _lectionLogService;
        private readonly ICrudService<Student> _studentService;
        private readonly ICrudService<Lection> _lectionService;
        private readonly ICrudService<Lector> _lectorService;
        private readonly IStudentNotificationService _notification;
        
        private readonly ILogger<LectionLogController> _logger;
        
        public LectionLogController(
            ILectionLogService lectionLogService, 
            ICrudService<Student> studentService, 
            ICrudService<Lection> lectionService,
            ICrudService<Lector> lectorService,
            IStudentNotificationService studentNotificationService,
            ILogger<LectionLogController> logger
            )
        {
            _lectionLogService = lectionLogService;
            _studentService = studentService;
            _lectionService = lectionService;
            _lectorService = lectorService;
            _notification = studentNotificationService;
            _logger = logger;
        }
        
        [HttpGet("{lectionId},{studentId},{homeworkId}")]
        public ActionResult<LectionLog> GetLog(int lectionId, int studentId, int homeworkId)
        {
            return _lectionLogService.Get(lectionId, studentId, homeworkId) switch
            {
                null => NotFound(),
                var log => log
            };
        }
        
        [HttpGet]
        public ActionResult<IReadOnlyCollection<LectionLog>> GetLogs()
        {
            return _lectionLogService.GetAll().ToArray();
        }

        [HttpPost]
        public IActionResult AddLog(LectionLog log)
        {
            try
            {
                var newLogId = _lectionLogService.New(log);
            
                _notification.Check(_studentService.Get(log.StudentId), _lectorService.Get(_lectionService.Get(log.LectionId).LectorId));
            
                return Ok($"api/lector_log/{newLogId},{newLogId},{newLogId}");
            }
            catch (Exception e)
            {
                _logger.LogError("Add Lection Log: There is already exist Lection Log with that ids");
                throw new AlreadyExistenceException("There is already exist Lection Log with that id");
            }
        }
        
        [HttpPut("{lectionId},{studentId},{homeworkId}")]
        public ActionResult<string> UpdateLog(int lectionId, int studentId, int homeworkId, LectionLog log)
        {
            try
            {
                var logId = _lectionLogService.Edit(log with 
                    { LectionId = lectionId, StudentId = studentId, HomeworkId = homeworkId});
            
                return Ok($"api/lector_log/{logId},{logId},{logId}");
            }
            catch (InvalidUserInputException e)
            {
                _logger.LogError("Edit Lection Log: There is no Lection Log with that id");
                throw;
            }
        }
        
        [HttpDelete("{lectionId},{studentId},{homeworkId}")]
        public ActionResult DeleteLog(int lectionId, int studentId, int homeworkId)
        {
            try
            {
                _lectionLogService.Delete(lectionId, studentId, homeworkId);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Delete Lection Log: There is no Lection Log with that id");
                throw new InvalidUserInputException("There is no Lection Log with that id");
            }
        }
        
    }
}