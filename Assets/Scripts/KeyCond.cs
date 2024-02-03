using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCond : MonoBehaviour
{
    private BoxCollider2D col;
    private SpriteRenderer sr;

    [SerializeField]
    private Sprite activeExit;
    [SerializeField] 
    private Sprite inactiveExit;

    [SerializeField]
    private int keyCount = 0;
    [SerializeField]
    private int playerHasKeys = 0;

    public int playerKeys
    {
        get { return playerHasKeys; }
        set { playerHasKeys = value; }
    }
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        if(keyCount == 0)
        {
            col.enabled = true;
            sr.sprite = activeExit;
        }
        else
        {
            col.enabled = false;
            sr.sprite = inactiveExit;
        }
    }
    void Update()
    {
        if(playerHasKeys == keyCount)
        {
            col.enabled = true;
            sr.sprite = activeExit;
        }
        else
        {
            col.enabled = false;
            sr.sprite = inactiveExit;
        }
    }
}
