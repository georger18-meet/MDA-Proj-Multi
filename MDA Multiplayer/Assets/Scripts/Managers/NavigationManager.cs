using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationManager : MonoBehaviour
{
    private NavMeshAgent _agent;
    private LineRenderer _lineRenderer;
    private bool _reachedDestination;

    [SerializeField] private EvacuationManager _evacuationManager;
    [SerializeField] private List<GameObject> listRoomEnums;
    [SerializeField] private Transform _destination;
    [SerializeField] private GameObject _destinationMarkerPrefab;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
    }

    void Update()
    {
        StopGPSNav();
    }

    public void StartGPSNav()
    {
        // EvacuationManager.Instance.DropDown_IndexChanged(index);
        Debug.Log(UIManager.Instance._dropDown.value); // Gives me the Enum value

        int enumRoom = UIManager.Instance._dropDown.value;

        for (int i = 0; i < listRoomEnums.Count; i++)
        {

            if (i == enumRoom)
            {
                _destinationMarkerPrefab.SetActive(true);
                //_destinationMarkerPrefab.transform.SetParent(listRoomEnums[i].transform);
                _destinationMarkerPrefab.transform.position =
                    listRoomEnums[i].transform.position + new Vector3(0f, 4f, 0f);
                _agent.SetDestination(listRoomEnums[i].transform.position);
                _agent.isStopped = true;
                _reachedDestination = false;
            }
        }
    }

    public void StopGPSNav()
    {
        if (Vector3.Distance(_agent.destination, transform.position) <= _agent.stoppingDistance)
        {
           // _destinationMarkerPrefab.transform.SetParent(transform);
            _destinationMarkerPrefab.SetActive(false);
            _reachedDestination = true;
        }
        else if (_agent.hasPath)
        {
            DrawPath();
        }
    }

    private void DrawPath()
    {
        if (!_reachedDestination)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.positionCount = _agent.path.corners.Length;
            _lineRenderer.SetPosition(0, transform.position);

            if (_agent.path.corners.Length < 2)
            {
                return;
            }

            for (int i = 1; i < _agent.path.corners.Length; i++)
            {
                var tempCornerList = _agent.path.corners;
                Vector3 pointPos = new Vector3(tempCornerList[i].x, tempCornerList[i].y, tempCornerList[i].z);
                _lineRenderer.SetPosition(i, pointPos);
            }
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
}
