using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInterface : MonoBehaviour
{
    [SerializeField] private GameObject commandMenu;
    [SerializeField] private int choiceCommand ;

    [Header("CommandMenu Sprites")] 
    [SerializeField] private Sprite[] commandMenuButtons = new Sprite[4];
    [SerializeField] private Sprite[] commandMenuButtonsSelected = new Sprite[4];

    void Start()
    {
        choiceCommand = 0;
        UpdateCommandMenu();
    }

    private void UpdateCommandMenu()
    {
        int i = 0;
        foreach (Transform button in commandMenu.transform)
        {
            if (i == choiceCommand) // Currently selected button
            {
                button.GetComponent<Image>().sprite = commandMenuButtonsSelected[i];
                //TODO place the heart soul at the button's position
            }
            else // Other selected buttons
            {
                button.GetComponent<Image>().sprite = commandMenuButtons[i];
            }
            i++;
        }
    } 

    /// <summary>
    /// Returns the BattleCommand (int) as a callback parameter
    /// FIGHT : 0
    /// ACT : 1
    /// ITEM : 2
    /// MERCY : 3
    /// </summary>
    /// <param name="result">Callback action with the BattleCommand as a result</param>
    /// <returns></returns>
    public IEnumerator CommandMenu(System.Action<int> result = null)
    {
        do
        {
            if (Input.GetKey(KeyCode.LeftArrow) && choiceCommand > 0)
            {
                choiceCommand--;
                UpdateCommandMenu();
                //TODO Play Scroll SFX
            }
            else if (Input.GetKey(KeyCode.RightArrow) && choiceCommand < 3)
            {
                choiceCommand++;
                UpdateCommandMenu();
                //TODO Play Scroll SFX
            }

            if (Input.GetKey(KeyCode.Space))
            {
                //TODO Play Select SFX
                if (result != null)
                {
                    Debug.Assert(choiceCommand is >= 0 and <= 3);
                    result(choiceCommand);
                }
                yield break;
            }
            
            yield return null;
        } while (true);
    }
}
