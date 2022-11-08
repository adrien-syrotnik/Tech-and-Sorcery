using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    private float health;

    public CanvasGroup bloodScreen;
    public GameObject bloodBorder;
    public GameObject bloodBG;
    public GameObject resetSceneUI;

    public Vector2 pitchAudio = new Vector2(0.8f, 1.2f);
    private AudioSource audioSource;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }


    public void TakeDamage(float damage)
    {
        if(isDead)
            return;
        health -= damage;
        audioSource.pitch = Random.Range(pitchAudio.x, pitchAudio.y);
        audioSource.Play();
        if (health <= 0)
        {
            Die();
        }
        else
        {
            bloodScreen.alpha = 1 - health / maxHealth;
        }
    }

    private void Die()
    {
        Time.timeScale = 0;
        resetSceneUI.SetActive(true);
        bloodBorder.SetActive(false);
        bloodBG.SetActive(true);
        isDead = true;
        //Destroy(gameObject);
    }



}
