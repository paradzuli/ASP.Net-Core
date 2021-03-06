﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreProject1.Controllers.Web;
using CoreProject1.Models;
using CoreProject1.Services;
using CoreProject1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreProject1.Controllers.Api
{
    [Authorize]
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController:Controller
    {
        private IWorldRepository _repository;
        private ILogger<StopsController> _logger;
        private GeoCoordsService _coordsService;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger,GeoCoordsService coordsService)
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordsService;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetUserTripByName(tripName,User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s=>s.Order).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get stops:{0}",ex);
                
            }return BadRequest($"Failed to get stops for trip {tripName}");
        }


        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody] StopViewModel vm)
        {
            try
            {
                //if the vm is valid
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);

                    //lookup the geocodes
                    var result = await _coordsService.GetCoordsAsync(newStop.Name);
                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;

                        //save to the database
                        _repository.AddStop(tripName, newStop, User.Identity.Name);
                        if (await _repository.SaveChangesAsync())
                        {
                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}",
                                Mapper.Map<StopViewModel>(newStop));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new stop : {0}",ex);
            }
            return BadRequest("Failed to save posts");
        }
       

    }
}
