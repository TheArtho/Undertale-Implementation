using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Most of the logic can be overriden in a extending class to match with the behaviour of the new enemy
/// </summary>
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
      public bool IsFainted => (Hp <= 0);

      protected EnemyAttack[] attacks;

      public Battle battle { get; private set; }
      public int battlerIndex { get; private set; }
      public int turnCount { get; private set; }
      
      #endregion
      
      #region Hardcoded Variables

      public string[] battleText { get; protected set; }
      
      #endregion

      public Enemy(string name, int hp)
      {
            this.name = name;
            maxHP = hp;
            this.Hp = hp;
            attacks = new EnemyAttack[] {new EnemyAttack()};
      }

      public void Initialize(Battle battle, int battlerIndex)
      {
            this.battle = battle;
            this.battlerIndex = battlerIndex;
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
            if (attacks.Length == 0) return -1;
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

      /// <summary>
      /// Faint animation
      /// </summary>
      /// <returns></returns>
      public virtual IEnumerator Faint()
      {
            battle.scene.battlers[battlerIndex].color = Color.clear;
            yield return null;
      }

      public virtual Color GetNameColor()
      {
            // Can be overriden for custom purposes
            return CanSpare() ? Color.yellow : Color.white;
      }

      public virtual string GetStartText()
      {
            // Custom text showing in the dialog box at the start of each turn
            return battleText[0] ?? "Please insert text";
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