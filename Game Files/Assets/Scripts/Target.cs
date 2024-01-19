using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private GameManager gameManager;
    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -2;

    public ParticleSystem explosionParticle;
    public int pointValue;

    private AudioSource slashAudio;
    private AudioSource bombHitAudio;
    public AnimationClip flashbangScreen;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

        transform.position = RandomSpawnPos();

        slashAudio = GameObject.Find("Slash Audio").GetComponent<AudioSource>();
        bombHitAudio = GameObject.Find("Bomb Hit Audio").GetComponent<AudioSource>();
        animator = GameObject.Find("Flashbang Effect").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if(gameManager.isGameActive && !gameManager.escaped)
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            gameManager.UpdateScore(pointValue);
            if(gameObject.CompareTag("Bad"))
            {
                bombHitAudio.Play();
                animator.Play(flashbangScreen.name);
                gameManager.livesLeft--;
                gameManager.UpdateLivesDisplay();
                if (gameManager.livesLeft <= 1)
                {
                    gameManager.GameOver();
                }
            }
            else
            {
                slashAudio.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if(!gameObject.CompareTag("Bad"))
        {
            if(gameManager.livesLeft > 1)
            {
                gameManager.livesLeft--;
                gameManager.UpdateLivesDisplay();
            }
            else if (gameManager.isGameActive && gameManager.livesLeft <= 1)
            {
                gameManager.GameOver();
            }
        }
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
}
