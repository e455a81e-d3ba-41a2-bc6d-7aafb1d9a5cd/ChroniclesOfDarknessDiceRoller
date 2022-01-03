using ChroniclesOfDarknessDiceRoller.Server.Data;
using ChroniclesOfDarknessDiceRoller.Server.Services;
using ChroniclesOfDarknessDiceRoller.Shared;
using ChroniclesOfDarknessDiceRoller.Shared.Constants;
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
                .OrderBy(t => t.Timestamp)
                .ToListAsync();

            var rolls = diceRolls.Select(t =>
            {
                var result = JsonSerializer.Deserialize<List<Roll>?>(t.ResultJson);
                return new RollResult(t.PlayerName, result);
            });
            await Clients.Client(Context.ConnectionId).SendAsync(SignalRMethods.ReceiveHistory, rolls);
        }

        public async Task RollDice(string game, string player, int amount, RerollType rerollType)
        {
            if (string.IsNullOrWhiteSpace(player))
            {
                await Clients.Client(Context.ConnectionId).SendAsync(SignalRMethods.ReceiveError, $"You need to provide a player name before rolling dice.");
                return;
            }
            if (string.IsNullOrWhiteSpace(game))
            {
                await Clients.Client(Context.ConnectionId).SendAsync(SignalRMethods.ReceiveError, $"You must join a game before you start rolling dice.");
                return;
            }
            if (amount >= DiceRollerConstants.DiceLimit)
            {
                await Clients.Client(Context.ConnectionId).SendAsync(SignalRMethods.ReceiveError, $"You are not allowed to roll more than {DiceRollerConstants.DiceLimit} dice.");
                return;
            }

            var result = _diceRoller.RollDice(amount, rerollType);
            _dbContext.DiceRolls.Add(new Model.DiceRoll
            {
                GameName = game,
                PlayerName = player,
                ResultJson = JsonSerializer.Serialize(result),
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            });
            await _dbContext.SaveChangesAsync();
            await Clients.Group(game).SendAsync(SignalRMethods.ReceiveDiceRoll, new RollResult(player, result));
        }
    }
}