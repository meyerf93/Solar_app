using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    private GameController gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if(gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if(gameController== null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter(Collider others)
    {
        if (others.tag == "Boundary") return;

        if(others.tag == "Player")
        {
            Instantiate(playerExplosion, others.transform.position, others.transform.rotation);
            gameController.GameOver();
        }

        Instantiate(explosion, transform.position, transform.rotation);

        gameController.AddScore(scoreValue);

        Destroy(others.gameObject);
        Destroy(gameObject);
    }
}
