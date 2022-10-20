using System.Collections;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionInterface : MonoBehaviour
{
    private int target;
    [SerializeField] public AudioDatabase audio;
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
            if (InputManager.Main.GetKeyDown(InputManager.Key.DownArrow) && target > 0)
            {
                AudioHandler.Main.PlaySFX(audio.Get("scroll"));
                target--;
                UpdateTargetSelection(opponents);
                //TODO Play Scroll SFX
                
                yield return new WaitForSeconds(0.1f);
            }
            else if (InputManager.Main.GetKeyDown(InputManager.Key.UpArrow) && target < opponents.Length)
            {
                AudioHandler.Main.PlaySFX(audio.Get("scroll"));
                target++;
                UpdateTargetSelection(opponents);
                //TODO Play Scroll SFX
                
                yield return new WaitForSeconds(0.1f);
            }

            if (InputManager.Main.GetKeyDown(InputManager.Key.Select))
            {
                AudioHandler.Main.PlaySFX(audio.Get("select"));
                if (result != null)
                {
                    Debug.Assert(target is >= -1 and <= 3);
                    result(target);
                }

                break;
            }
            if (InputManager.Main.GetKeyDown(InputManager.Key.Cancel))
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