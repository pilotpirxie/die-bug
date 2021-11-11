using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _minDistance = 10f;
    [SerializeField] private float _maxDistance = 50f;
    [SerializeField] private GameObject[] _players;
    [SerializeField] private float _zoomFactor = 1.5f;
    [SerializeField] private float _followTimeDelta = 0.8f;
    [SerializeField] private Camera _camera;

    public void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
    }

    public void FixedUpdate()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");

        Vector3 midpoint = Vector3.zero;
        Vector3 smallest = _players[0].transform.position;
        Vector3 greatest = _players[0].transform.position;

        foreach (GameObject player in _players)
        {
            midpoint += player.transform.position;

            smallest = Vector3.Min(smallest, player.transform.position);
            greatest = Vector3.Max(greatest, player.transform.position);
        }

        midpoint /= _players.Length;
        float distance = Math.Min(Math.Max(_minDistance, Vector3.Distance(smallest, greatest)), _maxDistance);

        Vector3 cameraDestination = midpoint - transform.forward * distance * _zoomFactor;
        if (_camera.orthographic) _camera.orthographicSize = distance;
        transform.position = Vector3.Slerp(transform.position, cameraDestination, _followTimeDelta);
        if ((cameraDestination - transform.position).magnitude <= 0.05f) transform.position = cameraDestination;
    }
}