using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class BulletTrail : MonoBehaviour
{
    public static BulletTrail i;
    TrailRenderer trail;
    Coroutine shooting;
    float speed = 400f; // meters per sec

    void Awake()
    {
        if (i != null) Debug.LogError($"More than one instance of {i} in scene");
        i = this;
    }
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
    }
    public void Shoot(Vector3 targetPoint)
    {
        if (shooting != null)
        {
            StopCoroutine(shooting);
        }
        shooting = StartCoroutine(ShootingCR(targetPoint));
    }

    IEnumerator ShootingCR(Vector3 targetPoint)
    {
        transform.position = Game.i.PlayerBulletOrigin.position;
        trail.enabled = true;

        while (Vector3.SqrMagnitude(targetPoint - transform.position) > 4f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
            yield return null;
        }

        trail.enabled = false;
        shooting = null;
    }
}
