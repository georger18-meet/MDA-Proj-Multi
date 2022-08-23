using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Pikud10 : MonoBehaviour
{
    private PhotonView _photonView => GetComponent<PhotonView>();
    private Coroutine _updatePlayerListCoroutine;

    //// x = x, y = z
    //[SerializeField] private Vector2 _markScale;
    private float _areaOffset = 18;

    [Header("Pikod10")]
    private GameObject _dropdownRefua10, _dropdownPinuy10, _dropdownHenyon10;
    public GameObject Pikod10Menu;
    public TMP_Dropdown PlayerListDropdownRefua10, PlayerListDropdownPinuy10, PlayerListDropdownHenyon10;
    public Button TopMenuHandle, AssignRefua10, AssignPinuy10, AssignHenyon10;
    public Button[] AllAreaMarkings = new Button[6];
    public bool IsPikud10MenuOpen;
    private LineRenderer _lineRenderer;
    private LayerMask _groundLayer;
    private Vector2 _targetPos;
    private float _targetHeight = 0.5f;
    private bool _isMarking = false;
    private CameraController _camController;
    //private int _currentMarkerIndex;

    #region MonobehaviourCallbacks
    private void Start()
    {
        InitializeMenu();
    }

    private void Update()
    {
        if (_isMarking)
            ChooseAreaPos(_camController);
    }
    #endregion

    #region Getters
    public int GetRefuaIndex()
    {
        int Index = 0;

        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            if (_dropdownRefua10.GetComponentInChildren<TextMeshProUGUI>().text == ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName)
            {
                Index = i;
                break;
            }

        }
        return Index;
    }
    public int GetPinoyeIndex()
    {
        int Index = 0;

        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            if (_dropdownPinuy10.GetComponentInChildren<TextMeshProUGUI>().text == ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName)
            {
                Index = i;
                break;
            }

        }
        return Index;
    }
    public int GetHenyonIndex()
    {
        int Index = 0;

        for (int i = 0; i < ActionsManager.Instance.AllPlayersPhotonViews.Count; i++)
        {
            if (_dropdownHenyon10.GetComponentInChildren<TextMeshProUGUI>().text == ActionsManager.Instance.AllPlayersPhotonViews[i].Owner.NickName)
            {
                Index = i;
                break;
            }

        }
        return Index;
    }
    #endregion

    #region Coroutines
    IEnumerator HandleDropDownUpdates(float nextUpdate)
    {
        while (true)
        {
            if (ActionsManager.Instance.AllPlayersPhotonViews.Count != PlayerListDropdownRefua10.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNamesPikud10", RpcTarget.AllBufferedViaServer);

            }
            if (ActionsManager.Instance.AllPlayersPhotonViews.Count != PlayerListDropdownPinuy10.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNamesPikud10", RpcTarget.AllBufferedViaServer);

            }
            if (ActionsManager.Instance.AllPlayersPhotonViews.Count != PlayerListDropdownHenyon10.options.Count)
            {
                _photonView.RPC("DropdownPlayersNickNamesPikud10", RpcTarget.AllBufferedViaServer);
            }
            yield return new WaitForSeconds(nextUpdate);
        }
    }
    #endregion

    private void InitializeMenu()
    {
        Pikod10Menu = UIManager.Instance.Pikud10Menu;
        TopMenuHandle = UIManager.Instance.Pikud10MenuHandle;
        TopMenuHandle.onClick.RemoveAllListeners();
        TopMenuHandle.onClick.AddListener(delegate { OpenClosePikud10Menu(); });

        _dropdownRefua10 = UIManager.Instance.DropdownRefua10;
        _dropdownPinuy10 = UIManager.Instance.DropdownPinuy10;
        _dropdownHenyon10 = UIManager.Instance.DropdownHenyon10;

        PlayerListDropdownRefua10 = UIManager.Instance.PlayerListDropdownRefua10;
        PlayerListDropdownPinuy10 = UIManager.Instance.PlayerListDropdownPinuy10;
        PlayerListDropdownHenyon10 = UIManager.Instance.PlayerListDropdownHenyon10;

        AssignRefua10 = UIManager.Instance.AssignRefua10;
        AssignPinuy10 = UIManager.Instance.AssignPinuy10;
        AssignHenyon10 = UIManager.Instance.AssignHenyon10;

        AllAreaMarkings[0] = UIManager.Instance.MarkUrgent;
        AllAreaMarkings[1] = UIManager.Instance.MarkUnUrgent;
        AllAreaMarkings[2] = UIManager.Instance.MarkVehicles;
        AllAreaMarkings[3] = UIManager.Instance.MarkGeneral;
        AllAreaMarkings[4] = UIManager.Instance.MarkDeceased;
        AllAreaMarkings[5] = UIManager.Instance.MarkBomb;

        AssignRefua10.onClick.RemoveAllListeners();
        AssignRefua10.onClick.AddListener(delegate { OnClickRefua(); });

        AssignPinuy10.onClick.RemoveAllListeners();
        AssignPinuy10.onClick.AddListener(delegate { OnClickPinoye(); });

        AssignHenyon10.onClick.RemoveAllListeners();
        AssignHenyon10.onClick.AddListener(delegate { OnClickHenyon(); });

        AllAreaMarkings[0].onClick.AddListener(delegate { OnClickMarker(0); });
        AllAreaMarkings[1].onClick.AddListener(delegate { OnClickMarker(1); });
        AllAreaMarkings[2].onClick.AddListener(delegate { OnClickMarker(2); });
        AllAreaMarkings[3].onClick.AddListener(delegate { OnClickMarker(3); });
        AllAreaMarkings[4].onClick.AddListener(delegate { OnClickMarker(4); });
        AllAreaMarkings[5].onClick.AddListener(delegate { OnClickMarker(5); });

        gameObject.AddComponent<LineRenderer>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 6;
        _lineRenderer.widthMultiplier = 0.1f;
        _groundLayer = LayerMask.GetMask("Ground");
        _groundLayer += LayerMask.GetMask("Road");
    }

    public void OpenClosePikud10Menu()
    {
        if (!IsPikud10MenuOpen)
        {
            UIManager.Instance.OpenCloseTopMenu("Pikud10");
            _updatePlayerListCoroutine = StartCoroutine(HandleDropDownUpdates(0.5f));
        }
        else
        {
            UIManager.Instance.OpenCloseTopMenu("Pikud10");
            StopCoroutine(_updatePlayerListCoroutine);
        }

        TopMenuHandle.onClick.RemoveAllListeners();
        TopMenuHandle.onClick.AddListener(delegate { OpenClosePikud10Menu(); });
    }

    public void OnClickRefua()
    {
        _photonView.RPC("GiveRefuaRole", RpcTarget.AllBufferedViaServer, GetRefuaIndex());
    }
    public void OnClickPinoye()
    {
        _photonView.RPC("GivePinoyeRole", RpcTarget.AllBufferedViaServer, GetPinoyeIndex());
    }
    public void OnClickHenyon()
    {
        _photonView.RPC("GiveHenyonRole", RpcTarget.AllBufferedViaServer, GetHenyonIndex());
    }
    public void OnClickMarker(int markerIndex) // markerIndex is responsible for choosing the targeted btn
    {
        _photonView.RPC("ActivateAreaMarkingRPC", RpcTarget.AllViaServer, markerIndex);
    }
    private void SetLineTargetPos()
    {
        _lineRenderer.SetPosition(0, new Vector3(_targetPos.x - _areaOffset, _targetHeight, _targetPos.y));
        _lineRenderer.SetPosition(1, new Vector3(_targetPos.x - _areaOffset, _targetHeight, _targetPos.y + _areaOffset));
        _lineRenderer.SetPosition(2, new Vector3(_targetPos.x + _areaOffset, _targetHeight, _targetPos.y + _areaOffset));
        _lineRenderer.SetPosition(3, new Vector3(_targetPos.x + _areaOffset, _targetHeight, _targetPos.y - _areaOffset));
        _lineRenderer.SetPosition(4, new Vector3(_targetPos.x - _areaOffset, _targetHeight, _targetPos.y - _areaOffset));
        _lineRenderer.SetPosition(5, new Vector3(_targetPos.x - _areaOffset, _targetHeight, _targetPos.y));

        _isMarking = false;
    }
    public void CreateMarkedArea(int markerIndex, CameraController camController)
    {
        _isMarking = true;
        //_currentMarkerIndex = markerIndex;
        _camController = camController;
        switch (markerIndex)
        {
            case 0:
                _lineRenderer.startColor = Color.red;
                _lineRenderer.endColor = Color.red;
                break;
            case 1:
                _lineRenderer.startColor = Color.green;
                _lineRenderer.endColor = Color.green;
                break;
            case 2:
                _lineRenderer.startColor = Color.blue;
                _lineRenderer.endColor = Color.yellow;
                break;
            case 3:
                _lineRenderer.startColor = Color.white;
                _lineRenderer.endColor = Color.white;
                break;
            case 4:
                _lineRenderer.startColor = Color.black;
                _lineRenderer.endColor = Color.black;
                break;
            case 5:
                _lineRenderer.startColor = Color.yellow;
                _lineRenderer.endColor = Color.red;
                break;
        }
    }
    private void ChooseAreaPos(CameraController camController)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camController.PlayerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit areaPosRaycastHit, 20f, _groundLayer))
            {
                _targetPos = areaPosRaycastHit.point;
                SetLineTargetPos();
            }
        }
    }
}
