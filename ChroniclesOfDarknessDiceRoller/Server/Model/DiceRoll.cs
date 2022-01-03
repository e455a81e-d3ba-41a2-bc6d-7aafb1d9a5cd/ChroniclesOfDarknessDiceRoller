namespace ChroniclesOfDarknessDiceRoller.Server.Model
{
    public class DiceRoll
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public string PlayerName { get; set; }
        public string ResultJson { get; set; }
        public long Timestamp { get; set; }
    }
}