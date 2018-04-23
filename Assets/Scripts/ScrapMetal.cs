using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapMetal : MonoBehaviour {

    [SerializeField] private int scrapValue = 1;

    private GameObject m_samuraiPlayer;

    private Vector3 m_directionToPlayer;

    private void Awake()
    {
        m_samuraiPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_samuraiPlayer.transform.position, 10 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2")
        {
            GameManager.reference.scrapMetal += scrapValue;
            GameManager.reference.waveScrap += scrapValue;
            Destroy(gameObject);
        }
    }
}
