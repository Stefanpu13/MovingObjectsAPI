using MovingObjects.Data;
using MovingObjects.Models;
using MovingObjects.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AuthenticationHelpers;

namespace MovingObjects.Controllers
{
    public class GameController : ApiController
    {
        private IGameRepository<Game> gameRepository;

        public GameController()
        {
            this.gameRepository = new GameRepository();
        }

        // get all games
        [HttpGet]
        [ActionName("All")]        
        public HttpResponseMessage GetGames(int playerId)
        {
            var player = gameRepository.GetPlayer(playerId);
            HttpResponseMessage response;

            if (player != null)
            {
                var gamesById =
                    this.gameRepository.GetAll().Where(g => g.PlayerId == playerId);

                if (!Token.ValidateToken(
                    Request.Headers.
                    FirstOrDefault(h => h.Key == "acces_token").Value.FirstOrDefault(),
                    player))
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                        "Can not see games of other player");
                }

                IEnumerable<GameModel> games = (
                    from game in gamesById
                    select new GameModel
                    {
                        Id = game.Id,
                        Name = game.Name,
                        SaveDate = game.Date,
                        GameObjects = (from gameObject in game.GameObject
                                       select new GameObjectModel
                                       {
                                           Color = gameObject.Color,
                                           ObjectId = gameObject.ObjectId,
                                           X = gameObject.X,
                                           Y = gameObject.Y,
                                           XDirection = gameObject.XDirection,
                                           YDirection = gameObject.YDirection
                                       }).ToList(),
                        GameState = new GameStateModel
                        {
                            Bonuses = game.GameState.Bonuses,
                            GameDifficulty = game.GameState.GameDifficulty,
                            GameLevel = game.GameState.GameLevel,
                            GameSpeed = game.GameState.GameSpeed,
                            Restarts = game.GameState.Restarts,
                            Score = game.GameState.Score
                        }
                    }
                    );

                response = Request.CreateResponse(HttpStatusCode.OK, games);
                
            }
            else 
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "PLayer not found.");
            }

            return response;
        }

        [HttpGet]
        [ActionName("Load")]
        public HttpResponseMessage LoadGame(int id, int playerId)
        {

            throw new NotImplementedException();
        }

        [HttpPost]
        [ActionName("Save")]
        public HttpResponseMessage SaveGame(int playerId, [FromBody] GameModel gameModel)
        {
            HttpResponseMessage response = null;
            if (gameModel == null)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    "Can not save empty game");
                return response;
            }
            else
            {
                Player player = gameRepository.GetPlayer(playerId);
                Game game = new Game();
                if (player != null)
                {
                    game.Date = gameModel.SaveDate;
                    game.Name = gameModel.Name;
                    game.Player = player;
                    game.GameState = new GameState
                    {
                        Bonuses = gameModel.GameState.Bonuses,
                        GameDifficulty = gameModel.GameState.GameDifficulty,
                        GameSpeed = gameModel.GameState.GameSpeed,
                        GameLevel = gameModel.GameState.GameLevel,
                        Restarts = gameModel.GameState.Restarts,
                        Score = gameModel.GameState.Score
                    };
                    // TODO: Add game objects.
                    foreach (var gameObject in gameModel.GameObjects)
                    {
                        game.GameObject.Add(new GameObject
                        {
                            ObjectId = gameObject.ObjectId,
                            X = gameObject.X,
                            Y = gameObject.Y,
                            XDirection = gameObject.XDirection,
                            YDirection = gameObject.YDirection,
                            Color = gameObject.Color
                        });
                    }

                    try
                    {
                        this.gameRepository.Add(game);
                    }
                    catch (Exception ex)
                    {
                        response =
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

                        return response;
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK,
                        "game added id: " + game.Id);

                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                        "Player with id " + playerId + " was not found.");
                }
            }

            return response;
        }

        [HttpDelete]
        [ActionName("Delete")]
        public HttpResponseMessage DeleteGame(int id, int playerId) 
        {
            HttpResponseMessage response = null;
            try
            {
                var player = this.gameRepository.GetPlayer(playerId);
                if (player != null)
                {
                    var game = player.Games.Where(g => g.Id == id).FirstOrDefault();
                    if (game != null)
                    {
                        gameRepository.Delete(game.Id);
                        response = Request.CreateResponse(HttpStatusCode.OK,
                        "Game with id: " + id + " deleted.");
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                            "The game was not found.");
                    }
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                        "Player not found");
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    ex);
            }

            return response;
        }
    }
}
