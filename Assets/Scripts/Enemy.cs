public class Enemy
{
      #region Parameters
      
      public string name { get; private set; }

      public int maxHP;

      private int _hp;
      public int Hp
      {
            get => maxHP;
            set
            {
                  if (value >= 0 && value <= maxHP)
                  {
                        _hp = value;
                  }
                  else
                  {
                        _hp = value < 0 ? 0 : maxHP;
                  }
            }
      }
      
      #endregion
      
      #region Hardcoded Variables

      public string[] battleText { get; private set; }
      
      #endregion

      public Enemy(string name, int hp)
      {
            this.name = name;
            maxHP = hp;
            this.Hp = hp;
            battleText = new[]
            {
                  string.Format("{0} came out of the earth!", name),
                  string.Format("{0}'s here for your health.", name),
                  string.Format("{0} seems kind of bruised", name)
            };
      }

      public void SetDamage(int damage)
      {
            _hp = _hp - damage;
      }
}