public class Player
{
        public string name { get; private set; }
        public int level { get; private set; }
        public int hp { get; private set; }
        public int maxHP { get; private set; }

        public Player(string name, int level)
        {
                this.name = name;
                this.level = level;
                maxHP = 24;     // hard coded value
                hp = maxHP;     // hard coded value
        }
}