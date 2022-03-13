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
    [Route("/api/lector")]
    public class LectorController : ControllerBase
    {
        private readonly ICrudService<Lector> _lectorService;
        private readonly ILogger<LectorController> _logger;
        
        public LectorController(ICrudService<Lector> lectorService, ILogger<LectorController> logger)
        {
            _lectorService = lectorService;
            _logger = logger;
        }
        
        [HttpGet("{id}")]
        public ActionResult<Lector> GetLector(int id)
        {
            return _lectorService.Get(id) switch
            {
                null => NotFound(),
                var lector => lector
            };
        }
        
        [HttpGet]
        public ActionResult<IReadOnlyCollection<Lector>> GetLectors()
        {
            return _lectorService.GetAll().ToArray();
        }

        [HttpPost]
        public IActionResult AddLector(Lector lector)
        {
            try
            {
                var newLectorId = _lectorService.New(lector);
                return Ok($"api/lector/{newLectorId}");
            }
            catch (Exception e)
            {
                _logger.LogError("Add Lector: There is already exist Lector with that id");
                throw new AlreadyExistenceException("There is already exist Lector with that id");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<string> UpdateLector(int id, Lector lector)
        {
            try
            {
                var lectorId = _lectorService.Edit(lector with { Id = id });
                return Ok($"api/lector/{lectorId}");
            }
            catch (InvalidUserInputException e)
            {
                _logger.LogError("Edit Lector: There is no Lector with that id");
                throw;
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteLector(int id)
        {
            try
            {
                _lectorService.Delete(id);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Delete Lector: There is no Lector with that id");
                throw new InvalidUserInputException("There is no Lector with that id");
            }
        }
    }
}