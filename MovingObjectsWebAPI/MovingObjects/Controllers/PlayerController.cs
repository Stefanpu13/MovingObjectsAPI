using MovingObjects.Data;
using MovingObjects.Models;
using MovingObjects.Repository;
using AuthenticationHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MovingObjects.Controllers
{
    public class PlayerController : ApiController
    {
        private PlayerRepository repository;

        public PlayerController()
        {
            this.repository = new PlayerRepository();
        }

        [HttpPost]
        [ActionName("Register")]
        public HttpResponseMessage Register([FromBody] PlayerModel playerModel) 
        {
            if (playerModel == null)
            {
                var response = Request
                    .CreateErrorResponse(HttpStatusCode.InternalServerError,
                    "Can not add empty player.");

                return response;
            }
            else
            {
                HttpResponseMessage response;
                // TODO: Check if username is unique
                if (!UserNameIsFree(playerModel.UserName))
                {
                    response = Request
                    .CreateErrorResponse(HttpStatusCode.Forbidden,
                    "Username already exists.");

                    return response;
                }

                // TODO: Validate password.
               var hash = PasswordHash.CreateHash(playerModel.Password);

                var player = new Player 
                {                    
                    UserName = playerModel.UserName,
                    // TODO: Use hashing to encrypt/decrypt password.
                    Password = hash,
                    Games = new HashSet<Game>()
                };

                try
                {
                    repository.Add(player);
                }
                catch (Exception e)
                {
                    response = Request
                       .CreateErrorResponse(HttpStatusCode.InternalServerError, e);

                    return response;
                }

                response = Request.CreateResponse(HttpStatusCode.Created, player.Id);
                response.Headers.Location =
                    new Uri(this.Request.RequestUri + "/" + player.Id.ToString());

                return response;
            }
        }

        [HttpPost]
        [ActionName("Login")]
        public HttpResponseMessage Login([FromBody] PlayerModel playerModel) 
        {
            HttpResponseMessage response;
            if (playerModel == null)
            {
                response = Request
                    .CreateErrorResponse(HttpStatusCode.InternalServerError,
                    "Can not login empty player.");
            }
            else
            {
                // Get the player from database
                Player player =  this.repository.Get(playerModel.UserName);
                // check  if 'player' password and 'playerModel' password are same
                var validPassword =  
                    PasswordHash.ValidatePassword(playerModel.Password, player.Password);

                if (validPassword)
                {
                    // Login
                    response = Request.CreateResponse(HttpStatusCode.Accepted,
                        "player loged in: " + player.Id);
                }
                else
                {
                    response = Request
                      .CreateErrorResponse(HttpStatusCode.InternalServerError,
                      "Username or password do not match");
                }
            }

            return response;
        }

        private bool UserNameIsFree(string userName) 
        {
            var isFree = 
                repository.
                GetAll().
                FirstOrDefault(p => p.UserName.ToLower() == userName.ToLower());
            return isFree == null;
        }
    }
}
