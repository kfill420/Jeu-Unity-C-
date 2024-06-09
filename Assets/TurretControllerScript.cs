using System.Collections;
using UnityEngine;

public class TurretControllerScript : MonoBehaviour
{
    public bool particleInProgress = false;
    private bool searching = true;
    public float vitesseRotation = 1f;
    public float force = 40f;
    public int damage = 1;
    public float delayReloading = 0.5f;

    public Transform turretTop;
    public AudioSource sourceSound;
    public Transform canonTransform;
    public ParticleSystem particleSystemBullet;
    public Light flashLight;

    public float raycastDistance = 20f;

    void Start()
    {
        //bulletParticleSystem.Pause();
        StartCoroutine(SearchTargetCoroutine());
    }

    void Update()
    {
        Vector3 objetPoint = canonTransform.transform.position;
        Vector3 rayDirection = -canonTransform.transform.right;
        Vector3 forwardOffset = rayDirection * 1f;
        Vector3 rayStartPoint = objetPoint + forwardOffset;


        RaycastHit hit;
        if (Physics.Raycast(rayStartPoint, rayDirection, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawLine(rayStartPoint, hit.point, Color.red);
                Tirer();
            }

        }
        else
        {
            Search();
            Debug.DrawRay(rayStartPoint, rayDirection * raycastDistance, Color.green);
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            StopCoroutine(SearchTargetCoroutine());
        }
    }

    public void Tirer()
    {
        sourceSound.Play();
        Vector3 centerPosition = canonTransform.TransformPoint(Vector3.zero);
        StopCoroutine(SearchTargetCoroutine());
        OnParticleSystemPlay();
        searching = false;
    }

    public void Search()
    {
        OnParticleSystemPause();
        searching = true;
    }

    IEnumerator SearchTargetCoroutine()
    {
        while (searching == true)
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
        yield return new WaitForSeconds(0.1f);
        flashLight.enabled = false;
    }
}