using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour {

    public float speed;
    public float speedUp;
    public float zoomSpeed;
    public float dampTime;
    public int deathCount;
    public float startingFuel = 100;
    public float currentFuel;
    public float fuelReduction;
    public float regenSpeed;

    public Vector3 spawnPosition;
    public Slider fuelSlider;
    public Text fuelText;
    public Text deathText;
    public Camera camera;

    private Vector3 mLastPosition;
    private Rigidbody rb;
    
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        deathCount = -1;
        SetDeathCount();
	}

    private void Awake()
    {
        currentFuel = startingFuel;
        SetFuelText();
    }

    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        
        
        Vector3 vertMovement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        rb.AddForce(vertMovement * speed);
        if (rb.IsSleeping())
        {
            if (currentFuel < startingFuel)
            {
                currentFuel = currentFuel + regenSpeed;
                if (currentFuel>startingFuel)
                {
                    currentFuel = startingFuel;
                }
            }
        }

        if (Input.GetButton("Jump"))
        {
            if (currentFuel > 0)
            {

                float moveVertical = Input.GetAxis("Vertical");


                Vector3 movement = new Vector3(0.0f, moveVertical, 0.0f);

                rb.AddForce(movement * speedUp);

                currentFuel = (currentFuel - fuelReduction);


            }
        }
        SetFuelText();
        fuelSlider.value = currentFuel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            FuelValue fuelValue = other.gameObject.GetComponent<FuelValue>();
            startingFuel = startingFuel + fuelValue.fuelGain;
            currentFuel = startingFuel;
            regenSpeed = regenSpeed + 3;

        if (camera)
        {

            float requiredSize = camera.orthographicSize + fuelValue.camChange;
            camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
        }

        }
        if (other.gameObject.CompareTag("Key"))
        {
            KeyValue keyValue = other.gameObject.GetComponent<KeyValue>();
             GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag (keyValue.color);
 
            foreach(GameObject go in gameObjectArray)
            {
                go.SetActive (false);
            }
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            rb.isKinematic = true;
            transform.position = spawnPosition;
            currentFuel = startingFuel;
            rb.isKinematic = false;
            SetDeathCount();
            Debug.Log("Obstacle");
        }
        if (other.gameObject.CompareTag("Respawn"))
        {
            Vector3 spawnpoint = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y-1, 0);
            spawnPosition = spawnpoint;
        }
         if (other.gameObject.CompareTag("L1Finish"))
        {
            SceneManager.LoadScene("Level 2");
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject player = GameObject.Find("Plane");
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        if (other.gameObject.CompareTag("Boundary"))
        {
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            rb.isKinematic = true;
            transform.position = playerMovement.spawnPosition;
            currentFuel = startingFuel;
            Debug.Log("Bound");
            rb.isKinematic = false;
            SetDeathCount();

        }

    }

    void SetFuelText()
    {
        int intFuel = (int) currentFuel;
        fuelText.text = "Fuel: " + intFuel.ToString();
    }

    void SetDeathCount()
    {
        deathCount = deathCount + 1;
        deathText.text = "Deaths: " + deathCount.ToString();
    }
}
