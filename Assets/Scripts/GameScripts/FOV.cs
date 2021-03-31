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

    public NavTank navtank;
    public ParticleSystem smoke;

    //[HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    private float nextTimeToFire = 0f;
    /*public void Start()
    {
        StartCoroutine("FindTargetWithDelay", .2f);
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            //findVisibleTargets();
        }
    }*/

    /*void findVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        if(visibleTargets.Count == 0){
            this.GetComponent<NavMeshAgent>().isStopped = false;
        }
        for (int i = 0; i < targetsInView.Length; i++)
        {
            if((targetsInView[i].gameObject != this.gameObject) || (targetsInView[i].tag == "Finish" && targetsInView[i].gameObject.layer != this.gameObject.layer)){
                
                Transform target = targetsInView[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    this.GetComponent<NavMeshAgent>().isStopped = true;
                    float disToTargets = Vector3.Distance(transform.position, target.position);
                    if ((!Physics.Raycast(transform.position, dirToTarget, disToTargets, obstacleMask) && target.gameObject.layer != this.gameObject.layer))
                    {
                        visibleTargets.Add(target);
                        if (Time.time >= nextTimeToFire)
                        {
                            nextTimeToFire = Time.time + 1f / fireRate;
                            shoot(dirToTarget);
                        }
                    }
                }            
            }
        }
    }*/

    public Vector3 DirFromAngle(float angleInDeg, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDeg += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
    }

    void shoot(Vector3 direction)
    {
        smoke.Play();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            NavTank enemyTank = hit.transform.GetComponent<NavTank>();
            if (enemyTank != null)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                if (enemyTank.TakeDamage(damage)){ //if the enemy is dead
                    this.transform.GetComponent<NavMeshAgent>().isStopped = false;
                }
                //Debug.Log("Damaged: " + hit.transform.name);
            }
        }
    }
}
