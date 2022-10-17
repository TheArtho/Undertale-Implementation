
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

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
    KILLED = 0,
    SPARED = 1,
    GAMEOVER = 2
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
  
  private BattleScene scene;

  private Player player;
  private Enemy[] opponents;

  private int turnCount;
  private BattleResult result;

  private BattleChoice lastChoice;
  
  #endregion

  public Battle(BattleScene scene, Player player, Enemy[] opponents)
  {
    this.scene = scene;
    this.player = player;
    this.opponents = opponents;
  }

  public void StartBattle()
  {
    if (opponents.Length > maxEnemy)
    {
      Debug.LogWarning("the number of enemies is too much (2 at max)");
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

    scene.StartCoroutine(DisplayMessage("Encounter Message"));
    
    #endregion
    
    #region Main Battle Part
    do
    {
      Debug.Log($"Turn #{turnCount+1}");
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
      
    } while (result == BattleResult.IN_PROGRESS);
    #endregion
    
    #region End of the Battle
    yield return scene.StartCoroutine(BattleEndPhase());
    #endregion
  }

  #region Battle Phase Coroutines

  private IEnumerator PlayerCommandPhase()
  {
    Debug.Log("[Battle] Player Command Phase");
    bool commandChosen = false;
    do
    {
      // Base Command Selection
      BattleCommand cmd = BattleCommand.FIGHT;

      yield return scene.StartCoroutine(scene.CommandMenu(value => cmd = (BattleCommand) value));

      Debug.Log($"Command chosen is {cmd}");

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
    int position = int.MaxValue;
    int damageToDeal;

    scene.SetActiveSoul(false);
    scene.ResetCommandMenu();

    yield return scene.StartCoroutine(scene.PlayerAttack(value => position = value));

    damageToDeal = CalculateDamage(position);
    
    Debug.Log($"Damage to deal = {damageToDeal}");

    yield return scene.StartCoroutine(DealDamage(lastChoice.target, damageToDeal));
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
    Debug.Log("[Battle] Enemy Attack Phase");
    throw new NotImplementedException();
  }

  /// <summary>
  /// End of turn phase
  /// </summary>
  /// <returns></returns>
  private IEnumerator EndTurnPhase()
  {
    Debug.Log("[Battle] End Turn Phase");
    throw new NotImplementedException();
  }

  /// <summary>
  /// End phase of the entire battle
  /// </summary>
  /// <returns></returns>
  private IEnumerator BattleEndPhase()
  {
    Debug.Log("[Battle] Battle End Phase");
    throw new NotImplementedException();
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

    opponent.SetDamage(damage);
    
    Debug.Log($"Enemy #{target} {opponent.name} : #{oldHp} => #{opponent.Hp}");
    
    yield return true;
  }
  
  #endregion
  
  #region Misc Coroutines

  /// <summary>
  /// Displays a message in the Dialog Box
  /// </summary>
  /// <returns></returns>
  private IEnumerator DisplayMessage(string message)
  {
    //TODO link the method to a dialog box of the battle scene
    Debug.Log(message);
    yield return null;
  }
  
  #endregion
  
  #region Methods
  
  /// <summary>
  /// Enemy AI attack choice
  /// </summary>
  /// <returns></returns>
  private void EnemyCommandPhase()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Triggers the Game Over screen
  /// </summary>
  private void GameOver()
  {
    scene.StopAllCoroutines();
  }

  private int CalculateDamage(int position)
  {
    return 25;
  }
  
  #endregion
}