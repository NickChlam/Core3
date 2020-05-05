using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
           
            var users = await _repo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users); // map from source to destination

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "getUser")]
        public async Task<IActionResult> getUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user); // map from source to destination
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser( UserForUpdateDto userForUpdate, int id) 
        {
            
            // ensure that the user is trying to edit thier own profile
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // get user 
            var userFromRepo = await _repo.GetUser(id);
            
            _mapper.Map(userForUpdate, userFromRepo);

            if (await _repo.SaveAll()) {
                  return NoContent();
            }
              
            
            throw new Exception($"Updating user {id} failed on save");



        }
    }
}