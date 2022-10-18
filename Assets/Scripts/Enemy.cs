using System;
using UnityEngine;

public class Enemy
{
      #region Parameters
      
      public string name { get; private set; }

      public int maxHP;

      private int _hp;
      public int Hp
      {
            get => _hp;
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
      
      public bool isSpared = false;
      public bool IsFainted => Hp <= 0;

      protected readonly EnemyAttack[] attacks;
      
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
            attacks = new EnemyAttack[]
            {
                  new Vegetoid_Attack_01(),
                  new Vegetoid_Attack_02()
            };
      }

      public void InitializeAttacks(Battle battle)
      {
            for (int i = 0; i < attacks.Length; ++i)
            {
                  attacks[i].Initialize(battle, this);
            }
      }

      public virtual int AttackChoice()
      {
            // Default attack choice is randomly picking from the attacks array
            return UnityEngine.Random.Range(0, attacks.Length);
      }
      
      public virtual bool CanSpare()
      {
            // Checks if the conditions to be spared are complete
            return false;
      }

      public virtual Color GetNameColor()
      {
            // Can be overriden for custom purposes
            return CanSpare() ? Color.yellow : Color.white;
      }

      public EnemyAttack GetAttack(int index)
      {
            Debug.Assert(index >= 0 && index < attacks.Length);
            return attacks[index];
      }
}