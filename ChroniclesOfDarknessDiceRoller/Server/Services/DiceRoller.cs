using ChroniclesOfDarknessDiceRoller.Shared;

namespace ChroniclesOfDarknessDiceRoller.Server.Services
{
    public class DiceRoller
    {
        private Random _d10;

        public DiceRoller()
        {
            _d10 = new Random();
        }

        public List<Roll> RollDice(int amount, RerollType rerollType)
        {
            var rolls = new List<Roll>();
            for (var i = 0; i < amount; i++)
            {
                var result = RollDie();
                var rerolls = new List<int>();
                var rerollThreshold = GetThreshold(rerollType);

                var lastResult = result;
                while (lastResult >= rerollThreshold)
                {
                    lastResult = RollDie();
                    rerolls.Add(lastResult);
                }

                rolls.Add(new Roll(result, rerolls));
            }
            return rolls.OrderBy(t => t.Result).ToList();
        }

        private int GetThreshold(RerollType rerollType)
        {
            return rerollType switch
            {
                RerollType.TenAgain => 10,
                RerollType.NoTenAgain => int.MaxValue,
                RerollType.NineAgain => 9,
                RerollType.EightAgain => 8,
                _ => throw new NotImplementedException(),
            };
        }

        private int RollDie()
        {
            return _d10.Next(1, 11);
        }
    }
}