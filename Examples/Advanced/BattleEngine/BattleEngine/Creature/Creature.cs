
namespace BattleEngine
{
    internal class Creature : Renderable
    {
        public string CreatureName { get; internal set; }

        public string Description { get; internal set; } 

        public string MainSprite { get; internal set; }

        public int Id { get; internal set; }

        internal int MaxHealth { get; set; }

        public int Health { get; set; }

        public Creature(string name, int id, string description, string mainSprite, int maxHealth) : base()
        {
            CreatureName = name;
            Id = id;
            Description = description;
            MainSprite = mainSprite;
            MaxHealth = maxHealth;
        }
    }
}
