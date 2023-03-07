
namespace BattleEngine
{
    internal class Move
    {
        internal string Name { get; set; }

        internal int Power { get; set; }

        internal int MaxPower { get; set; }

        public Move(string name, int power, int maxPower)
        {
            Name = name;
            Power = power;
            MaxPower = maxPower;
        }
    }
}
