using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FOV : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    [Range(0, 100)]
    public float damage = 10;
    public float fireRate = 15f;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public LayerMask ignoreRaycast;

    //public TankNav navtank;
    //public ParticleSystem smoke;

    //[HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    public bool found = false;
    bool foundTank = false;
    public Transform barrel;
    private float nextTimeToFire = 0.0f;
    public void Start()
    {
        StartCoroutine("FindTargetWithDelay", .2f);
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            findVisibleTargets();
        }
    }

    void findVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        foundTank = false;

        for (int i = 0; i < targetsInView.Length; i++)
        {
            // Om den inte ser sig sj�lv eller sin fabrik
            if ((targetsInView[i].gameObject != this.gameObject) || (targetsInView[i].tag == "Finish" && targetsInView[i].GetComponent<TankHealth>().team != this.gameObject.GetComponent<TankHealth>().team))
            {

                Transform target = targetsInView[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {

                    foundTank = true;
                    float disToTargets = Vector3.Distance(transform.position, target.position);
                    if ((!Physics.Raycast(transform.position, dirToTarget, disToTargets, obstacleMask) && targetsInView[i].GetComponent<TankHealth>().team != this.gameObject.GetComponent<TankHealth>().team))
                    {
                        visibleTargets.Add(target);
                        if (Time.time >= nextTimeToFire)
                        {
                            Debug.Log(target.name);
                            nextTimeToFire = Time.time + 1f / fireRate;
                            shoot(dirToTarget, target);
                        }
                    }
                }
            }
        }
        if (foundTank)
            found = true;
        else
            found = false;
    }

    public Vector3 DirFromAngle(float angleInDeg, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDeg += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
    }

    void shoot(Vector3 direction, Transform target)
    {
        //smoke.Play();
        RaycastHit hit;


        if (Physics.Raycast(barrel.position, direction, out hit))
        {
            if (hit.collider.gameObject != gameObject)
            {
                TankHealth enemyTank = hit.transform.GetComponent<TankHealth>();
                TankNav tankNav = GetComponent<TankNav>();

                
                this.transform.LookAt(hit.transform);
                Debug.Log("Hit: " + hit.collider.name);
                Debug.Log("Enemy: " + enemyTank.name);

                if (enemyTank != null)
                {
                    nextTimeToFire = Time.time + 1f / fireRate;
                    if (enemyTank.TakeDamage(damage))
                    {
                        found = false;
                        if(tankNav != null)  
                            tankNav.SetRotation();
                        
                    } //if the enemy is dead
                    //Debug.Log("Damaged: " + hit.transform.name);
                }
            }
        }
    }
}
