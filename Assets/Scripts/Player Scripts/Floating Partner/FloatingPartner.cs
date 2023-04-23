using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPartner : MonoBehaviour
{
    public Transform target;
    public float maxMoveSpeed = 5f;
    public float minMoveSpeed = 1f;
    public float hoverRadius = 1f;

    private float hoverTime = 0f;
    private Vector3 targetPosition;
    private bool isHovering = false;

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float speed = Mathf.Lerp(minMoveSpeed, maxMoveSpeed, distanceToTarget / maxMoveSpeed);

        if (distanceToTarget > 0.1f)
        {
            if (isHovering)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
                hoverTime -= Time.deltaTime;

                if (hoverTime <= 0f)
                {
                    isHovering = false;
                }
            }
            else
            {
                Vector3 direction = (target.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.deltaTime);

                if (distanceToTarget < hoverRadius)
                {
                    targetPosition = target.position + Random.insideUnitSphere * hoverRadius;
                    isHovering = true;
                    hoverTime = Random.Range(1f, 1.5f);
                }
            }
        }
    }
}
