using ChroniclesOfDarknessDiceRoller.Server.Data;
using ChroniclesOfDarknessDiceRoller.Server.Services;
using ChroniclesOfDarknessDiceRoller.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ChroniclesOfDarknessDiceRoller.Server.Hubs
{
    public class DiceRollerHub : Hub
    {
        private readonly DiceRoller _diceRoller;
        private readonly DiceRollerContext _dbContext;

        private readonly Dictionary<string, string> _playersToGameMap = new();

        public DiceRollerHub(DiceRoller diceRoller, DiceRollerContext context)
        {
            _diceRoller = diceRoller;
            _dbContext = context;
        }

        public async Task JoinGame(string game)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, game);

            var diceRolls = await _dbContext.DiceRolls
                .Where(t => t.GameName == game)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();

            var rolls = diceRolls.Select(t =>
            {
                var result = JsonSerializer.Deserialize<List<Roll>?>(t.ResultJson);
                return new RollResult(t.PlayerName, result);
            });
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveHistory", rolls);
        }

        public async Task RollDice(string game, string player, int amount, RerollType rerollType)
        {
            var result = _diceRoller.RollDice(amount, rerollType);
            _dbContext.DiceRolls.Add(new Model.DiceRoll
            {
                GameName = game,
                PlayerName = player,
                ResultJson = JsonSerializer.Serialize(result),
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            });
            await _dbContext.SaveChangesAsync();
            await Clients.Group(game).SendAsync("ReceiveDiceRoll", new RollResult(player, result));
        }
    }
}