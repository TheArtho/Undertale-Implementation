using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleScene : MonoBehaviour
{
    #region Constants

    private readonly float boxTransformTime = 0.5f;
    private readonly Vector2 defaultBoxSize = new Vector2(575, 140);
    private readonly Vector2 defaultBoxPosition = new Vector2(0, -81);

    #endregion
    
    private Battle battle;
    [SerializeField] public AudioDatabase audio;
    [FormerlySerializedAs("dialogBox")] public Image battleBox;
    [SerializeField] private GameObject battleInterface;
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
        yield return new WaitForSeconds(0.05f);
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
        yield return new WaitForSeconds(0.05f);
    }

    public IEnumerator StartPlayerAttack(System.Action<int> result)
    {
        yield return StartCoroutine(playerAttackHandler.DefaultAttack(result));
        yield return new WaitForSeconds(0.05f);
    }

    public IEnumerator HideAttackMeter()
    {
        yield return StartCoroutine(playerAttackHandler.DismissAttackMeter());
    }

    public void SetActiveSoul(bool value)
    {
        soul.gameObject.SetActive(value);
    }

    public void CenterSoul()
    {
        soul.rectTransform.position = battleBox.rectTransform.position;
    }

    public void ToggleBattleSoul(bool value)
    {
        if (value)
        {
            soul.transform.SetParent(battleBox.transform.Find("Mask/DodgeArea"));
        }
        else
        {
            soul.transform.SetParent(battleInterface.transform);
        }
    }

    public void ResetCommandMenu()
    {
        commandMenuInterface.ResetMenu();
    }

    public IEnumerator SetBoxSize(Vector2 size, float time)
    {
        LeanTween.size(battleBox.rectTransform, size, time);
        yield return new WaitForSeconds(time);
    }

    public IEnumerator ResetBoxSize()
    {
        yield return StartCoroutine(SetBoxSize(defaultBoxSize, boxTransformTime));
    }
    
    public IEnumerator SetBoxPosition(Vector2 position, float time)
    {
        LeanTween.moveLocal(battleBox.gameObject, position, time);
        yield return new WaitForSeconds(time);
    }
    
    public IEnumerator ResetBoxPosition()
    {
        yield return  StartCoroutine(SetBoxPosition(defaultBoxPosition, boxTransformTime));
    }

    public void MoveSoul(bool value)
    {
        soul.GetComponent<SoulController>().canMove = value;
    }
}
