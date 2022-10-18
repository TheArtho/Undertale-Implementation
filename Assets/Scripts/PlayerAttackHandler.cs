using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackHandler : MonoBehaviour
{
    private RectTransform _rectTransform;
    
    [SerializeField] private RectTransform dialogBox;
    [SerializeField] private GameObject attackBar;

    private bool isRunning = false;
    private int waitInputId;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        GetComponent<Image>().color = Color.clear;
    }

    private void Update()
    {
        // Makes the attack meter size dependent of the dialog box
        _rectTransform.sizeDelta = new Vector2(dialogBox.sizeDelta.x - 29, dialogBox.sizeDelta.y - 25);
    }

    private bool GetAttackInput(RectTransform attackBarRect)
    {
        return Input.GetKeyDown(KeyCode.Return) && attackBarRect.localPosition.x >= -275 &&
               attackBarRect.localPosition.x <= 275;
    }

    private int RemapPosition(float position)
    {
        return Mathf.RoundToInt(math.remap(-275, 275, -100, 100, position));
    }

    public IEnumerator DefaultAttack(System.Action<int> result)
    {
        // The attack system might chance depending on the weapon of the player, so this code should be called from an external function
        // Default Attack system
        attackBar.SetActive(true);
        
        GetComponent<Image>().color = Color.white;
        
        Coroutine waitInput = StartCoroutine(WaitInput());
        RectTransform attackBarRect = attackBar.GetComponent<RectTransform>();

        while (isRunning && !GetAttackInput(attackBarRect))
        {
            yield return null;
        }
        
        LeanTween.cancel(waitInputId);
        StopCoroutine(waitInput);
        
        result(RemapPosition(attackBarRect.localPosition.x));
    }

    private IEnumerator WaitInput()
    {
        isRunning = true;
        LeanTween.moveLocalX(attackBar, -335, 0);
        waitInputId = LeanTween.moveLocalX(attackBar, 335, 2).id;
        yield return new WaitForSeconds(2);
        isRunning = false;
    }

    public IEnumerator DismissAttackMeter()
    {
        attackBar.SetActive(false);
        GetComponent<Image>().color = Color.clear;
        yield return 0.5f;
    }
}
