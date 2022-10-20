using System;
using System.Collections;
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

      public Battle battle  { get; private set; }
      public int turnCount { get; private set; }
      
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

      public void Initialize(Battle battle)
      {
            this.battle = battle;
            InitializeAttacks(battle);
      }

      private void InitializeAttacks(Battle battle)
      {
            for (int i = 0; i < attacks.Length; ++i)
            {
                  attacks[i].Initialize(battle, this);
            }
      }

      /// <summary>
      /// Chooses an attack depending on the situation
      /// </summary>
      /// <returns></returns>
      public virtual int AttackChoice()
      {
            // Default attack choice is randomly picking from the attacks array
            return UnityEngine.Random.Range(0, attacks.Length);
      }

      /// <summary>
      /// Checks if the conditions to be spared are complete
      /// </summary>
      /// <returns></returns>
      public virtual bool CanSpare()
      {
            // Checks if the conditions to be spared are complete
            return false;
      }
      
      /// <summary>
      /// Custom Behavior depending on the situation
      /// </summary>
      /// <returns></returns>
      public virtual IEnumerator StartTurnBehaviour()
      {
            // Custom Behavior depending on the situation
            yield return null;
      }

      /// <summary>
      /// Additional behavior at the end of the turn
      /// </summary>
      /// <returns></returns>
      public virtual IEnumerator EndTurnBehavior()
      {
            // Additional effects for the Enemy at the end of the turn
            yield return null;
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

      public void UpdateTurn()
      {
            turnCount = battle.turnCount;
      }
}