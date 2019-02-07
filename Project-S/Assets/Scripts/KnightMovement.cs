using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : KnightParts
{
    readonly int moveFrame = 10;
    bool isMoving = false;

    public void MoveToPoint(Vector3 goal) {
        if (isMoving) return;
        StartCoroutine(MoveToPointCoroutine(goal));
    }


    IEnumerator MoveToPointCoroutine(Vector3 goal) {
        isMoving = true;
        Vector3 diff = goal - core.status.pos;
        for(int i = 0; i < moveFrame * Mathf.Abs(diff.x); i++) {
            transform.position += Vector3.right * 10 / moveFrame * (diff.x > 0 ? 1 : -1);
            yield return null;
        }
        for (int i = 0; i < moveFrame * Mathf.Abs(diff.z); i++) {
            transform.position += Vector3.forward * 10 / moveFrame * (diff.z > 0 ? 1 : -1);
            yield return null;
        }
        core.status.pos = goal;
        isMoving = false;
    }

}
