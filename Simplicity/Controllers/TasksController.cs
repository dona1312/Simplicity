﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Simplicity.Entities;
using Simplicity.Helpers;
using Simplicity.Services.ServicesInterfaces;
using Simplicity.ViewModels.Tasks;

namespace Simplicity.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITicketsService _tasksService;
        private readonly IMapper _mapper;
        private IHubContext<LocationHub, ILocationHubService> _hubContext;
        private readonly IUsersService _usersService;

        public TasksController(ITicketsService tasksService,
            IMapper mapper, IHubContext<LocationHub, ILocationHubService> hubContext,
            IUsersService usersService)
        {
            _tasksService = tasksService;
            _mapper = mapper;
            _hubContext = hubContext;
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _tasksService.GetAllTaskDtos(x => true);
            return Ok(result);
        }

        [HttpGet("getByUserID/{id}")]
        public IActionResult GetByUserID(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var result = _tasksService.GetAllTaskDtos(x => x.AssigneeID == id);
            return Ok(result);
        }

        [HttpGet("{id}", Name = "tasks/getByID")]
        public IActionResult GetByID(int id)
        {
            var model = _tasksService.GetAll(x => x.ID == id).FirstOrDefault();

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpPost]
        public IActionResult Post([FromForm] TasksEditVM model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var entity = new Ticket();
            _mapper.Map(model, entity);

            var changes = PrepareChanges(model);

            //map users to projects here
            if (!_tasksService.Save(entity))
                return StatusCode(500);


            _hubContext.Clients.All.GetMessage(changes);
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ticket = _tasksService.GetById(id);
            if (ticket == null)
            {
                return NotFound();
            }
            var ticketName = ticket.Name;

            var isProjectDeleted = _tasksService.Delete(id);
            if (!isProjectDeleted)
            {
                return StatusCode(500);
            }

            _hubContext.Clients.All.GetMessage($"{ticketName} was deleted");
            
            return Ok();
        }

        private string PrepareChanges(TasksEditVM model)
        {
            var action = "was updated";

            if (model.Status.ToString() != model.OldStatus.ToString())
            {
                action = $@"updated status from {model.OldStatus.GetDescription()}
                            to {model.Status.GetDescription()}";
            }
            
            return $"'{model.Name}' {action}";
        }
    }
}