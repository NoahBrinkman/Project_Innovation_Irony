using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerLobbyHandler : MonoBehaviour
{

    [SerializeField] private RectTransform miningTransform;
    [SerializeField] private RectTransform cleaningTransform;
    [SerializeField] private RectTransform smeltingTransform;
    [SerializeField] private RectTransform castingTransform;
    [SerializeField] private float playerChoseThisYValueRenameThisLaterPlease;
    [SerializeField] private float noPlayerYValue;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveUp();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            moveDown();
        }
    }

    private void MoveUp()
    {
        miningTransform.DOMoveY(playerChoseThisYValueRenameThisLaterPlease, 1).SetEase(Ease.OutBounce);
    }

    private void moveDown()
    {
        miningTransform.DOMoveY(noPlayerYValue, 1).SetEase(Ease.InQuad);
    }
}



