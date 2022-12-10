using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class SetNavigationTarget : MonoBehaviour
{
   [SerializeField] private Camera _topDownCamera;
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
    }
}
