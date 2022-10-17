
using System;
using System.Collections;
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
  
  private BattleScene scene;

  private Player player;
  private Monster[] opponents;

  private int turnCount;
  private BattleResult result;

  private BattleCommand lastCommand;

  public Battle(BattleScene scene, Player player, Monster[] opponents)
  {
    this.scene = scene;
    this.player = player;
    this.opponents = opponents;
  }

  public void StartBattle()
  {
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
      yield return scene.StartCoroutine(PlayerCommandPhase());
      switch (lastCommand)
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
    BattleCommand cmd = BattleCommand.FIGHT;

    yield return scene.StartCoroutine(scene.CommandMenu(value => cmd = (BattleCommand) value));
    lastCommand = cmd;
    
    Debug.Log($"Command chosen is {cmd}");

    if (cmd == BattleCommand.FIGHT)
    {
      
    }
    else if (cmd == BattleCommand.ACT)
    {
      
    }
    else if (cmd == BattleCommand.ITEM)
    {
      
    }
    else if (cmd == BattleCommand.MERCY)
    {
      
    }
  }

  /// <summary>
  /// Player RPG-styled Attack phase
  /// </summary>
  /// <returns></returns>
  private IEnumerator PlayerAttackPhase()
  {
    throw new NotImplementedException();
  }
  
  /// <summary>
  /// Player chose an Act
  /// </summary>
  /// <returns></returns>
  private IEnumerator PlayerActPhase()
  {
    throw new NotImplementedException();
  }
  
  /// <summary>
  /// Player chose a Mercy option
  /// </summary>
  /// <returns></returns>
  private IEnumerator PlayerMercyPhase()
  {
    throw new NotImplementedException();
  }
  
  /// <summary>
  /// Player chose to use an Item
  /// </summary>
  /// <returns></returns>
  private IEnumerator PlayerItemPhase()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Enemy BulletHell-styled Attack phase
  /// </summary>
  /// <returns></returns>
  private IEnumerator EnemyAttackPhase()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// End of turn phase
  /// </summary>
  /// <returns></returns>
  private IEnumerator EndTurnPhase()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// End phase of the entire battle
  /// </summary>
  /// <returns></returns>
  private IEnumerator BattleEndPhase()
  {
    throw new NotImplementedException();
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
  
  #endregion
}