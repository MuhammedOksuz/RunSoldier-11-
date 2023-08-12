using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float transition;
    [SerializeField] Vector3 distance;
    [SerializeField] Vector3 rotationDistance;
    public void FollowCamera()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position - distance, transition);
        transform.LookAt(player.transform.position - rotationDistance);
    }
}
