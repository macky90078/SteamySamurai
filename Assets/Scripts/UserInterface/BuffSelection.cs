using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class BuffSelection : MonoBehaviour {

    private enum buffType { attackBuff, moveBuff, healthBuff }
    [SerializeField] private buffType buff;
    [SerializeField] private Text buttonText;
    [SerializeField] private float moveSpeedInc = 10;
    [SerializeField] private float attkSpeedInc = 10;
    [SerializeField]
    private int attackBuffCost = 1;
    [SerializeField]
    private int moveBuffCost = 1;

    private bool buffPicked = false;
    public GameObject[] players;

	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        SetText();
    }

    public void attackBuff()
    {
        GameObject player = findPlayer(GameManager.reference.buffIndex);
        PlayerController x = player.GetComponent<PlayerController>();
        if(x.hasAttackBuff == false)
        {
            x.buffAttack(attkSpeedInc);
            x.scrapMetal -= attackBuffCost;
            GameManager.reference.buffIndex++;
        }
    }

    public void healthBuff()
    {
        GameObject player = findPlayer(GameManager.reference.buffIndex);
        PlayerController x = player.GetComponent<PlayerController>();
        x.buffHealth();
        GameManager.reference.buffIndex++;
    }

    public void moveBuff()
    {
        GameObject player = findPlayer(GameManager.reference.buffIndex);
        PlayerController x = player.GetComponent<PlayerController>();
        if(x.hasMoveBuff == false)
        {
            x.buffSpeed(moveSpeedInc);
            x.scrapMetal -= moveBuffCost;
            GameManager.reference.buffIndex++;
        }
    }

    void SetText()
    {
        switch (buff)
        {
            case buffType.attackBuff:
                buttonText.text = buttonText.text + "Cost: " + attackBuffCost;
                break;
            case buffType.moveBuff:
                buttonText.text = buttonText.text + "Cost: " + moveBuffCost;
                break;
            case buffType.healthBuff:
                buttonText.text = buttonText.text + "Free";
                break;
        }
    }

    public GameObject findPlayer(int id)
    {
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PlayerController>().playerId == id)
                return players[i];
        }
        return null;
    }
}
