using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollisionScript : MonoBehaviour
{
    private PlayerCharacterScript _player;

    // Use this for initialization
    void Start()
    {
        _player = GameObject.FindObjectOfType<PlayerCharacterScript>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Rock"))
        {
            Debug.Log("Collision with Rock!");
            _player.OnRockCollision();
        }
    }
}
