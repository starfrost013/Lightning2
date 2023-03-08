
namespace BattleEngine
{
    internal class Move
    {
        internal string Name { get; set; }

        internal int Damage { get; set; }

        internal int MaxDamage { get; set; }

        public Move(string name, int damage, int maxDamage)
        {
            Name = name;
            Damage = damage;
            MaxDamage = maxDamage;
        }
    }
}
