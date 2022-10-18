using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// CommandMenu
public class CommandMenuInterface : MonoBehaviour
{
    [SerializeField] private int choiceCommand ;
    [SerializeField] private Image soul;

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
        foreach (Transform button in transform)
        {
            if (i == choiceCommand) // Currently selected button
            {
                button.GetComponent<Image>().sprite = commandMenuButtonsSelected[i];
                soul.rectTransform.position = button.GetComponent<RectTransform>().position;
            }
            else // Other selected buttons
            {
                button.GetComponent<Image>().sprite = commandMenuButtons[i];
            }
            i++;
        }
    }

    public void ResetMenu()
    {
        int i = 0;
        foreach (Transform button in transform)
        {
            button.GetComponent<Image>().sprite = commandMenuButtons[i];
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
        UpdateCommandMenu();
        soul.gameObject.SetActive(true);
        
        do
        {
            if (InputManager.Main.GetKeyDown(InputManager.Key.LeftArrow))
            {
                choiceCommand--;

                if (choiceCommand < 0) choiceCommand = 3;
                
                UpdateCommandMenu();
                //TODO Play Scroll SFX

                yield return new WaitForSeconds(0.1f);
            }
            else if (InputManager.Main.GetKeyDown(InputManager.Key.RightArrow))
            {
                choiceCommand++;
                
                if (choiceCommand > 3) choiceCommand = 0;
                
                UpdateCommandMenu();
                //TODO Play Scroll SFX
                
                yield return new WaitForSeconds(0.1f);
            }

            if (InputManager.Main.GetKeyDown(InputManager.Key.Select))
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
