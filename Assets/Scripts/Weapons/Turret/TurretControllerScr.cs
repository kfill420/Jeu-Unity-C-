using System.Collections;
using UnityEngine;

public class TurretControllerScr : MonoBehaviour
{
    public bool playerFounded = false;
    private bool searching = true;
    public float vitesseRotation = 1f;
    public float force = 40f;
    public int damage = 1;
    public float delayReloading = 0.1f;

    private bool readyToFire = true;

    IEnumerator myCoroutine;

    private TriggerEnterScript triggerEnterScript;
    public Collider coneDetection;
    public Transform turretTop;
    public AudioSource sourceSound;
    public Transform canonTransform;
    public ParticleSystem particleSystemBullet;
    public Light flashLight;

    //public float raycastDistance = 20f;

    void Start()
    {
        /*Debug.Log("TurretTop test");
        turretTop = transform.Find("turretTop");
        sourceSound = transform.Find("sound").GetComponent<AudioSource>();
        canonTransform = transform.Find("Canon").GetComponent<Transform>();
        particleSystemBullet = transform.Find("Particle System").GetComponent<ParticleSystem>();*/

        triggerEnterScript = GetComponentInChildren<TriggerEnterScript>();
        if (triggerEnterScript != null)
        {
            Debug.Log("Script trigger trouvé");
        }
        else
        {
            Debug.Log("Script trigger no ntrouvé");
        }

        myCoroutine = SearchTargetCoroutine();
        StartCoroutine(myCoroutine);
    }

    void Update()
    {
        Vector3 objetPoint = canonTransform.transform.position;
        Vector3 rayDirection = -canonTransform.transform.right;
        Vector3 forwardOffset = rayDirection * 1f;
        Vector3 rayStartPoint = objetPoint + forwardOffset;

        // Technique avec Raycast
        /*RaycastHit hit;
        if (Physics.Raycast(rayStartPoint, rayDirection, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerFounded = true;
                Debug.DrawLine(rayStartPoint, hit.point, Color.red);
                Tirer();
                playerFounded = false;
            }
            else if (!hit.collider.CompareTag("Player"))
            {
                searching = true;
                Search();
                Debug.DrawRay(rayStartPoint, rayDirection * raycastDistance, Color.green);
                searching = false;
            }

        }*/

        if (triggerEnterScript != null && triggerEnterScript.trigger)
        {
            searching = false;
            Tirer();
        }
        else if (!searching)
        {
            searching = true;
            Search();
        }
    }

    public void Tirer()
    {
        if (readyToFire)
        {
            sourceSound.Play();
            StopCoroutine(myCoroutine);
            OnParticleSystemPlay();
            StartCoroutine(WaitNextBullet());
        }

    }

    public void Search()
    {
        StartCoroutine(myCoroutine);
    }

    IEnumerator SearchTargetCoroutine()
    {
        searching = true;
        while (true)
        {
            Quaternion startRotation = turretTop.rotation;
            Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, 180);

            float timeCount = 0.0f;

            while (timeCount < 1.0f)
            {
                timeCount += Time.deltaTime * vitesseRotation;
                turretTop.rotation = Quaternion.Slerp(startRotation, endRotation, timeCount);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);


            startRotation = turretTop.rotation;
            endRotation = startRotation * Quaternion.Euler(0, 0, -180);


            timeCount = 0.0f;

            while (timeCount < 1.0f)
            {
                timeCount += Time.deltaTime * vitesseRotation;
                turretTop.rotation = Quaternion.Slerp(startRotation, endRotation, timeCount);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator WaitNextBullet()
    {
        readyToFire = false;
        yield return new WaitForSeconds(delayReloading);
        readyToFire = true;
    }


    public void OnParticleSystemPlay()
    {
        particleSystemBullet.Play();
        StartCoroutine(OnLightPlay());
    }

    public void OnParticleSystemPause()
    {
        particleSystemBullet.Pause();
        StopCoroutine(OnLightPlay());
    }

    IEnumerator OnLightPlay()
    {
        flashLight.enabled = true;
        yield return new WaitForSeconds(0.05f);
        flashLight.enabled = false;
    }
}