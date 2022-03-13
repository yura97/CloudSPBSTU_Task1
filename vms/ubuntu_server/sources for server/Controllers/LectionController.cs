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
    [Route("/api/lection")]
    public class LectionController : ControllerBase
    {
        private readonly ICrudService<Lection> _lectionService;
        private readonly ILogger<LectionController> _logger;
        
        public LectionController(ICrudService<Lection> lectionService, ILogger<LectionController> logger)
        {
            _lectionService = lectionService;
            _logger = logger;
        }
        
        [HttpGet("{id}")]
        public ActionResult<Lection> GetLection(int id)
        {
            return _lectionService.Get(id) switch
            {
                null => NotFound(),
                var lection => lection
            };
        }
        
        [HttpGet]
        public ActionResult<IReadOnlyCollection<Lection>> GetLections()
        {
            return _lectionService.GetAll().ToArray();
        }

        [HttpPost]
        public IActionResult AddLection(Lection lection)
        {
            try
            {
                var newLectionId = _lectionService.New(lection);
                return Ok($"api/lection/{newLectionId}");
            }
            catch (Exception e)
            {
                _logger.LogError("Add Lection: There is already exist Lection with that id");
                throw new AlreadyExistenceException("There is already exist Lection with that id");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<string> UpdateLection(int id, Lection lection)
        {
            try
            {
                var lectionId = _lectionService.Edit(lection with { Id = id });
                return Ok($"api/lection/{lectionId}");
            }
            catch (InvalidUserInputException e)
            {
                _logger.LogError("Edit Lection: There is no lection with that id");
                throw;
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteLection(int id)
        {
            try
            {
                _lectionService.Delete(id);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Delete Lection: There is no lection with that id");
                throw new InvalidUserInputException("There is no lection with that id");
            }
        }
    }
}