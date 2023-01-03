using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown navigationTargetDropdown;
    [SerializeField] private List<Target> navigationTargets = new List<Target>();

    private NavMeshPath _path;
    private LineRenderer _line;
    private Vector3 _targetPosition = Vector3.zero;

    private bool _lineToggle = false;
    
    private void Start()
    {
        _path = new NavMeshPath();
        _line = transform.GetComponent<LineRenderer>();
        _line.enabled = _lineToggle;
    }

    private void Update()
    {
        if (_lineToggle && _targetPosition != Vector3.zero)
        {
            NavMesh.CalculatePath(transform.position, _targetPosition, NavMesh.AllAreas, _path);
            _line.positionCount = _path.corners.Length;
            _line.SetPositions(_path.corners);
        }
    }

    public void SetCurrentNavigationTarget(int selectedValue)
    {
        _targetPosition = Vector3.zero;
        var selectedText = navigationTargetDropdown.options[selectedValue].text;
        var currentTarget = navigationTargets.Find(t =>
            t.Name.Equals(selectedText, StringComparison.InvariantCultureIgnoreCase));
        if (currentTarget != null)
        {
            _targetPosition = currentTarget.transform.position;
        }
    }

    public void ChangeToggleVisibility()
    {
        _lineToggle = !_lineToggle;
        _line.enabled = _lineToggle;
    }

    /*[SerializeField] private Camera _topDownCamera;
     [SerializeField] private GameObject _navTargerObject;
 
     private NavMeshPath _path;
     private LineRenderer _line;
     private bool lineToggle = false;
     
     private void Start()
     {
         _path = new NavMeshPath();
         _line = transform.GetComponent<LineRenderer>();
     }
 
     private void Update()
     {
         if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
         {
             lineToggle = !lineToggle;
         }
 
         if (lineToggle)
         {
             NavMesh.CalculatePath(transform.position, _navTargerObject.transform.position, NavMesh.AllAreas, _path);
             _line.positionCount = _path.corners.Length;
             _line.SetPositions(_path.corners);
             _line.enabled = true;
         }
     }*/
}
