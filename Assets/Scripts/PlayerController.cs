using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 5.0f;
    private GameObject focalPoint;
    public bool hasPowerUp;
    private float powerupForce = 15.0f;
    public GameObject powerupIndicator;
    public AudioClip bumpingSoundHigh;
    public AudioClip bumpingSoundLow;
    public AudioClip powerupSound;
    private AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerupIndicator.gameObject.transform.position = transform.position + new Vector3(0, -.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            playerAudio.PlayOneShot(powerupSound);
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        playerAudio.PlayOneShot(bumpingSoundLow);

        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 forceDirection = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(forceDirection * powerupForce, ForceMode.Impulse);
            playerAudio.PlayOneShot(bumpingSoundHigh);

            Debug.Log("Collided with: " + collision.gameObject.name + " with powerup set to: " + hasPowerUp);
        }
    }
}
