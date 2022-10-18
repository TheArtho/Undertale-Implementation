using System.Collections;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionInterface : MonoBehaviour
{
    private int target;
    [SerializeField] private Image soul;
    
    private void Start()
    {
        target = 0;
    }

    private void SetupTargets(Enemy[] opponents)
    {
        int i = 0;
        foreach (Transform button in transform)
        {
            if (i >= opponents.Length)
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);
                button.Find("Name").GetComponent<Text>().text = $"* {opponents[i].name}";
                button.Find("Name").GetComponent<Text>().color = opponents[i].GetNameColor();
                button.Find("HP Bar").GetComponent<Slider>().maxValue = opponents[i].maxHP;
                button.Find("HP Bar").GetComponent<Slider>().value = opponents[i].Hp;
            }

            i++;
        }
    }
    
    private void UpdateTargetSelection(Enemy[] opponents)
    {
        int i = 0;
        foreach (Transform button in transform)
        {
            if (i >= opponents.Length) break;
            if (i == target) // Currently selected button
            {
                soul.rectTransform.position = button.GetComponent<RectTransform>().position;
            }
            i++;
        }
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
        gameObject.SetActive(true);
        SetupTargets(opponents);
        UpdateTargetSelection(opponents);
        
        do
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) && target > 0)
            {
                target--;
                UpdateTargetSelection(opponents);
                //TODO Play Scroll SFX
                
                yield return new WaitForSeconds(0.1f);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && target < opponents.Length)
            {
                target++;
                UpdateTargetSelection(opponents);
                //TODO Play Scroll SFX
                
                yield return new WaitForSeconds(0.1f);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                //TODO Play Select SFX
                if (result != null)
                {
                    Debug.Assert(target is >= -1 and <= 3);
                    result(target);
                }

                break;
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //TODO Play Cancel SFX
                if (result != null)
                {
                    result(-1);
                }
                break;
            }
            
            yield return null;
        } while (true);
        
        gameObject.SetActive(false);
    }
}