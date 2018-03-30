using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBuffText : MonoBehaviour {

    [SerializeField]
    private Text playerText;
    [SerializeField]
    private Text scrapText;
    [SerializeField]
    private GameObject anyBuffButton;

    private int id;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        id = GameManager.reference.buffIndex;
        playerText.text = "Player " + (id+1);
        scrapText.text = anyBuffButton.GetComponent<BuffSelection>().findPlayer(id).GetComponent<PlayerController>().scrapMetal + "\nScrap\nMetal";
	}
}
