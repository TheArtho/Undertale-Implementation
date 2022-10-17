using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleScene : MonoBehaviour
{
    private Battle battle;
    [SerializeField] private CommandMenuInterface commandMenuInterface;
    [SerializeField] private TargetSelectionInterface targetSelectionInterfaceInterface;
    [SerializeField] private PlayerAttackHandler playerAttackHandler;
    
    [SerializeField] private Image soul;
    
    // Start is called before the first frame update
    void Start()
    {
        Player p = new Player("Chara", 2);
        Enemy m = new Enemy("Vegetoid", 70);

        battle = new Battle(this, p, new[] {m});
        
        battle.StartBattle();
    }
    
    // BattleScene Coroutines

    /// <summary>
    /// Returns the BattleCommand (int) as a callback parameter
    /// FIGHT : 0
    /// ACT : 1
    /// ITEM : 2
    /// MERCY : 3
    /// </summary>
    /// <param name="result">Callback action with the BattleCommand as a result</param>
    /// <returns></returns>
    public IEnumerator CommandMenu(System.Action<int> result)
    {
        yield return StartCoroutine(commandMenuInterface.CommandMenu(result));
        yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// Returns an int as a callback parameter
    /// -1 : CANCEL
    /// 0 to 3 : target (in the order of the opponent array of the Battle class)
    /// </summary>
    /// /// <param name="opponents">Enemies to choose</param>
    /// <param name="result">Callback action with an int as a result</param>
    /// <returns></returns>
    public IEnumerator TargetSelection(Enemy[] opponents, System.Action<int> result = null)
    {
        yield return StartCoroutine(targetSelectionInterfaceInterface.TargetSelection(opponents, result));
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator PlayerAttack(System.Action<int> result)
    {
        yield return StartCoroutine(playerAttackHandler.DefaultAttack(result));
        yield return new WaitForEndOfFrame();
    }

    public void SetActiveSoul(bool value)
    {
        soul.gameObject.SetActive(value);
    }

    public void ResetCommandMenu()
    {
        commandMenuInterface.ResetMenu();
    }
}
