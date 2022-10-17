using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    private Battle battle;
    [SerializeField] private BattleInterface battleInterface;
    
    // Start is called before the first frame update
    void Start()
    {
        Player p = new Player("Chara", 2);
        Monster m = new Monster("Vegetoid", 70);

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
        yield return StartCoroutine(battleInterface.CommandMenu(result));
    }
}
