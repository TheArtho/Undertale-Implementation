
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

public class Battle
{
  #region Semantic Enumerations

  public enum BattleCommand
  {
    FIGHT = 0,
    ACT = 1,
    ITEM = 2,
    MERCY = 3
  }
  
  public enum BattleResult
  {
    IN_PROGRESS = -1,
    LOST = 0,
    WON = 1
  }
  
  #endregion

  #region Data Structures

  private struct PlayerChoice
  {
    public int command;
    public int target;
  }

  #endregion
  
  #region Parameters

  private const int maxEnemy = 2;
  
  public BattleScene scene { get; private set; }

  private Player player;
  private Enemy[] opponents;

  public int turnCount { get; private set; }
  private BattleResult result;

  private BattleChoice lastChoice;
  private int[] enemiesChoice;
  
  #endregion

  #region Constructors
  public Battle(BattleScene scene, Player player, Enemy[] opponents)
  {
    this.scene = scene;
    this.player = player;
    this.opponents = opponents;

    for (int i = 0; i < opponents.Length; ++i)
    {
      opponents[i].Initialize(this, i);
    }
  }
  #endregion

  public void StartBattle()
  {
    if (CountOpponentsActive() > maxEnemy)
    {
      Debug.LogWarning("The number of enemies is higher than 2");
    }
    
    Debug.Log($"[Battle] Starting battle: {player.name} against {opponents[0]}");
    result = BattleResult.IN_PROGRESS;
    try
    {
      scene.StartCoroutine(BattleCore());
    }
    catch (Exception e)
    {
      Debug.LogException(e);
    }
  }
  
  //
  // Coroutines
  //

  private IEnumerator BattleCore()
  {
    #region Initialization
    
    
    
    #endregion
    
    #region Main Battle Part
    do
    {
      Debug.Log($"Turn #{turnCount+1}");

      scene.ResetDialogBox();
      scene.DrawDialogBox();
      yield return scene.StartCoroutine(DisplayMessage($"* {opponents[0].battleText[turnCount % opponents[0].battleText.Length]}"));
      
      yield return scene.StartCoroutine(PlayerCommandPhase());

      switch (lastChoice.command)
      {
        case BattleCommand.FIGHT:
          yield return scene.StartCoroutine(PlayerAttackPhase());
          break;
        case BattleCommand.ACT:
          yield return scene.StartCoroutine(PlayerActPhase());
          break;
        case BattleCommand.ITEM:
          yield return scene.StartCoroutine(PlayerItemPhase());
          break;
        case BattleCommand.MERCY:
          yield return scene.StartCoroutine(PlayerMercyPhase());
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      EnemyCommandPhase();
      yield return scene.StartCoroutine(EnemyAttackPhase());
      yield return scene.StartCoroutine(EndTurnPhase());
      // Going to the next turn
      turnCount++;
    } while (result == BattleResult.IN_PROGRESS);
    #endregion
    
    #region End of the Battle
    yield return scene.StartCoroutine(BattleEndPhase());
    Debug.Log($"You {result}");
    #endregion
  }

  #region Battle Phase Coroutines

  private IEnumerator PlayerCommandPhase()
  {
    Debug.Log("[Battle] Player Command Phase");
    bool commandChosen = false;
    do
    {
      scene.DrawDialogBox();
      
      // Base Command Selection
      BattleCommand cmd = BattleCommand.FIGHT;

      yield return scene.StartCoroutine(scene.CommandMenu(value => cmd = (BattleCommand) value));

      Debug.Log($"Command chosen is {cmd}");

      scene.UndrawDialogBox();
      
      if (cmd == BattleCommand.FIGHT)
      {
        // Target Selection
        int target = 0;
      
        yield return scene.StartCoroutine(scene.TargetSelection(opponents, value => target = value));

        if (target == -1) {
          Debug.Log("CANCELLING");
          continue; // CANCEL
        }

        lastChoice = new BattleFightChoice(target);
        commandChosen = true;
      }
      else if (cmd == BattleCommand.ACT)
      {
        throw new NotImplementedException();
      }
      else if (cmd == BattleCommand.ITEM)
      {
        throw new NotImplementedException();
      }
      else if (cmd == BattleCommand.MERCY)
      {
        throw new NotImplementedException();
      }
    } while (!commandChosen);
  }

  /// <summary>
  /// Player RPG-styled Attack phase
  /// </summary>
  /// <returns></returns>
  private IEnumerator PlayerAttackPhase()
  {
    Debug.Log("[Battle] Player Attack Phase");
    int position = 0;
    int damageToDeal;

    scene.SetActiveSoul(false);
    scene.ResetCommandMenu();

    yield return scene.StartCoroutine(scene.StartPlayerAttack(value => position = value));

    damageToDeal = CalculateDamage(position);
    
    Debug.Log($"Damage to deal = {damageToDeal}");

    yield return scene.StartCoroutine(DealDamage(lastChoice.target, damageToDeal));

    scene.StartCoroutine(scene.HideAttackMeter());

    if (opponents[lastChoice.target].IsFainted)
    {
      yield return scene.StartCoroutine( opponents[lastChoice.target].Faint());
    }
  }
  
  /// <summary>
  /// Player chose an Act
  /// </summary>
  /// <returns></returns>
  private IEnumerator PlayerActPhase()
  {
    Debug.Log("[Battle] Player Act Phase");
    throw new NotImplementedException();
  }
  
  /// <summary>
  /// Player chose a Mercy option
  /// </summary>
  /// <returns></returns>
  private IEnumerator PlayerMercyPhase()
  {
    Debug.Log("[Battle] Player Mercy Phase");
    throw new NotImplementedException();
  }
  
  /// <summary>
  /// Player chose to use an Item
  /// </summary>
  /// <returns></returns>
  private IEnumerator PlayerItemPhase()
  {
    Debug.Log("[Battle] Player Item Phase");
    throw new NotImplementedException();
  }

  /// <summary>
  /// Enemy BulletHell-styled Attack phase
  /// </summary>
  /// <returns></returns>
  private IEnumerator EnemyAttackPhase()
  {
    // Only implementing the attack phase of the first enemy for now
    Debug.Log("[Battle] Enemy Attack Phase");
    
    if (CheckBattleState()) yield break;

    int enemyIndex = 0;
    EnemyAttack attack = opponents[enemyIndex].GetAttack(enemiesChoice[enemyIndex]);

    Debug.Log($"{opponents[enemyIndex]} starts {attack}");

    yield return scene.StartCoroutine(attack.Use());
    
    scene.SetActiveSoul(false);
    scene.StartCoroutine(scene.ResetBoxPosition());
    yield return scene.StartCoroutine(scene.ResetBoxSize());
  }

  /// <summary>
  /// End of turn phase
  /// </summary>
  /// <returns></returns>
  private IEnumerator EndTurnPhase()
  {
    Debug.Log("[Battle] End Turn Phase");
    yield return null;
  }

  /// <summary>
  /// End phase of the entire battle
  /// </summary>
  /// <returns></returns>
  private IEnumerator BattleEndPhase()
  {
    Debug.Log("[Battle] Battle End Phase");

    if (result == BattleResult.WON)
    {
      scene.StopBGM();
      
      scene.ResetDialogBox();
      scene.DrawDialogBox();
      yield return scene.StartCoroutine(DisplayMessageAndWait("* You WON!\n* You earned 6 XP and 1 gold."));
    }
    else
    {
      yield return null;
    }
  }
  
  #endregion
  
  #region Sub Phase Coroutines

  /// <summary>
  /// Deals the input damage to the target
  /// </summary>
  /// <param name="target"></param>
  /// <param name="damage"></param>
  /// <returns></returns>
  private IEnumerator DealDamage(int target, int damage)
  {
    Enemy opponent = opponents[target];
    int oldHp = opponent.Hp;

    // Attack Animation
    // Displays the health bar
    // Damage animation

    opponent.Hp -= damage;

    Debug.Log($"Enemy #{target} {opponent.name} : #{oldHp} => #{opponent.Hp}");
    
    yield return null;
  }
  
  #endregion
  
  #region Misc Coroutines

  /// <summary>
  /// Displays a message in the Dialog Box
  /// </summary>
  /// <returns></returns>
  private IEnumerator DisplayMessage(string message)
  {
    Debug.Log(message);
    yield return scene.StartCoroutine(scene.DisplayMessage(message));
  }

  private IEnumerator DisplayMessageAndWait(string message)
  {
    Debug.Log(message);
    yield return scene.StartCoroutine(scene.DisplayMessageAndWait(message));
  }
  
  #endregion
  
  #region Methods
  
  /// <summary>
  /// Enemy AI attack choice
  /// </summary>
  /// <returns></returns>
  private void EnemyCommandPhase()
  {
    if (CheckBattleState()) return;
    enemiesChoice = new int[opponents.Length];
    for (int i = 0; i < opponents.Length; ++i)
    {
      // For each opponent, choose an attack
      Enemy currentOpponent = opponents[i];
      Debug.Assert(currentOpponent != null);

      if (currentOpponent.IsFainted || currentOpponent.isSpared) continue;
      
      enemiesChoice[i] = currentOpponent.AttackChoice();
    }
  }

  /// <summary>
  /// Checks if the battle has to over and update the battle result
  /// </summary>
  private bool CheckBattleState()
  {
    if (CountOpponentsActive() == 0)
    {
      result = BattleResult.WON;
      return true;
    }

    return false;
  }

  /// <summary>
  /// Triggers the Game Over screen
  /// </summary>
  private void GameOver()
  {
    scene.StopAllCoroutines();
  }
  
  // Algo Methods

  private int CalculateDamage(int position)
  {
    return 25;
  }

  private int CountOpponentsActive()
  {
    int amount = 0;
    for (int i = 0; i < opponents.Length; ++i)
    {
      Debug.Assert(opponents[i] != null);
      if (!opponents[i].IsFainted && !opponents[i].isSpared)
      {
        amount++;
      }
    }
    return amount;
  }
  
  #endregion
}