namespace Combat.Enemies
{
    public class Vegetoid : Enemy
    {
        public Vegetoid(string name, int hp) : base(name, hp)
        {
            battleText = new[]
            {
                string.Format("{0} came out of the earth!", name),
                string.Format("{0}'s here for your health.", name),
                string.Format("{0} seems kind of bruised", name)
            };
            attacks = new EnemyAttack[]
            {
                new Vegetoid_Attack_01(),
                new Vegetoid_Attack_02()
            };
        }
    }
}