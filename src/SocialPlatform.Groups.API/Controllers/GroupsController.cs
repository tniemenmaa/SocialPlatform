using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors;
using SocialPlatform.Groups.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialPlatform.Groups.Shared.Models;

namespace SocialPlatform.Groups.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        public GroupsController( IGroupRegistryService groupService, ILogger<GroupsController> logger )
        {
            _groupService = groupService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IEnumerable<Group>> Get()
        {
            return (await _groupService.GetGroups());
        }

        [HttpGet("{id}")]
        public async Task<Group> GetGroup(Guid id)
        {
            ///<remarks>
            /// In real world this would be done within database query engine
            /// added here for api completeness
            /// </remarks>
            return (await _groupService.GetGroups()).FirstOrDefault(x => x.Id == id);
        }

        [HttpGet("{id}/messages")]
        public async Task<IEnumerable<GroupMessage>> GetGroupMessages(Guid id)
        {
            IGroupActor actor = ActorProxy.Create<IGroupActor>(new ActorId(id), Constants.GroupActorUri);
            return await actor.GetMessages();
        }

        private readonly IGroupRegistryService _groupService;
        private readonly ILogger<GroupsController> _logger;
    }
}
