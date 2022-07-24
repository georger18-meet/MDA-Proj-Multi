using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform _playerUI;
    [SerializeField] private GameObject _toolTipPrefab;
    [SerializeField] private TextMeshProUGUI _toolTipTextMesh;
    [SerializeField] private string _toolTipContent;

    private GameObject _newToolTip;
    private const int _half = 2;
    private bool _mouseHover = false;

    void Update()
    {
        if (_mouseHover && !_newToolTip)
        {
            Debug.Log("Mouse Hover");

            
        }
        else
        {
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseHover = true;
        Debug.Log("Mouse Hover");

        float yNewPos = gameObject.transform.localScale.y / _half;
        float xNewPos = gameObject.transform.localScale.x / _half;
        Vector3 newPos = new Vector3(xNewPos, yNewPos, transform.position.z);

        _newToolTip = Instantiate(_toolTipPrefab, newPos, Quaternion.identity, transform);
        //_newToolTip.transform.position = newPos;
        _toolTipTextMesh.text = _toolTipContent;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseHover = false;
        Debug.Log("Mouse Hover");

        _toolTipTextMesh.text = null;
        Destroy(_newToolTip);
    }
}
