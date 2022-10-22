namespace Combat.Enemies
{
    /// <summary>
    /// Vegetoid class extending from the Enemy class
    /// Most of the logic can be overriden to match with the behaviour of the new enemy
    /// </summary>
    public class Vegetoid : Enemy
    {
        public Vegetoid(string name, int hp) : base(name, hp)
        {
            // string.Format is very useful for localizations
            battleText = new[]
            {
                string.Format("{0} came out of the earth!", name),
                string.Format("{0}'s here for your\n  health.", name),
                string.Format("{0} seems kind of\n  bruised", name)
            };
            attacks = new EnemyAttack[]
            {
                new Vegetoid_Attack_01(),
                new Vegetoid_Attack_02()
            };
        }

        public override bool CanSpare()
        {
            // Can be spared if its HP are low enough
            return Hp < maxHP / 2;
        }

        public override string GetStartText()
        {
            // If HP low enough return a specific text
            if (Hp < maxHP / 2)
            {
                return battleText[3];
            }
            // Otherwise, return one of the two first texts
            else
            {
                return battleText[turnCount % 2];
            }
        }
    }
}