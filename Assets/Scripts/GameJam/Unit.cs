using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;

    public Transform target;
    public float speed = 20;
    public float turnSpeed = 3;
    public float turnDst = 5;
    public float stoppingDst = 10;

    private Vector3 m_currentPos;
    private Quaternion m_currentRot;

    Path path;

    [SerializeField] GameObject m_childRefObj;
    [SerializeField] OldGameManager m_gameManagerObj;

  public bool m_isFrozen = false;
  public bool m_isNorm = true;

  public bool m_isDead = false;

    [SerializeField] private Material m_mattFrozen;
    [SerializeField] private Material m_mattNorm;

    [SerializeField] private GameObject m_changeMattObj;

    private float m_freezeCoolDown = 5f;

    // Update is called once per frame
    void Update()
    {
        m_currentPos = transform.position;
        m_currentRot = transform.rotation;

        if (!m_isDead)
        {
            if (m_isNorm && m_childRefObj.GetComponent<CheckForPlayer>().m_bPlayerInRange == true)
            {
                m_freezeCoolDown = 5f;
                m_changeMattObj.GetComponent<Renderer>().material.color = m_mattNorm.color;
                StartCoroutine(UpdatePath());
                m_isNorm = false;
            }
            else if (m_isFrozen)
            {
                m_isNorm = false;
                StopAllCoroutines();
                m_freezeCoolDown -= Time.deltaTime;
                m_changeMattObj.GetComponent<Renderer>().material.color = m_mattFrozen.color;
                if (m_freezeCoolDown <= 0f)
                {
                    m_isNorm = true;
                    m_isFrozen = false;
                }
            }
        }

        if (m_isDead)
        {
            m_gameManagerObj.GetComponent<OldGameManager>().m_deadEnemyLastPos = m_currentPos;
            m_gameManagerObj.GetComponent<OldGameManager>().m_deadEnemyLastRot = m_currentRot;

            m_gameManagerObj.GetComponent<OldGameManager>().m_enemyDied = true;
            Destroy(gameObject);
        }

    }



    void Start()
    {
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDst, stoppingDst);

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {

        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            //print(((target.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {

        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {

                if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }

                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
            }

            yield return null;

        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}