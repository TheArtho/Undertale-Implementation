public class Monster
{
      #region Parameters
      
      public string name { get; private set; }

      public int maxHP;
      public int hp
      {
            get => hp;
            set { hp = value < 0 ? 0 : value; }
      }
      
      #endregion
      
      #region Hardcoded Variables

      public string[] battleText { get; private set; }
      
      #endregion

      public Monster(string name, int hp)
      {
            this.name = name;
            maxHP = hp;
            this.hp = hp;
            battleText = new[]
            {
                  string.Format("{0} came out of the earth!", name),
                  string.Format("{0}'s here for your health.", name),
                  string.Format("{0} seems kind of bruised", name)
            };
      }
}