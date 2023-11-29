using UnityEngine;

public class Commander : MonoBehaviour
{

    [SerializeField] private Monster[] controlledMonsters;
    
    private float _rotationDirection;
    private Vector3 _moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        InputManager.Initialize(this);
    }

    private void Update()
    {
        Transform myTransCached  = transform;
        myTransCached.Rotate(Vector3.up,Time.deltaTime *   Settings.Instance.MouseRotateSens * _rotationDirection );
        myTransCached.position += transform.rotation * (Time.deltaTime * Settings.Instance.MouseMoveSense * _moveDirection);
    }

    public void Attack(Ray camToWorldRay)
    {
        Debug.DrawRay(camToWorldRay.origin, camToWorldRay.direction * 100, Color.red,1);
        if(!Physics.Raycast(camToWorldRay, out RaycastHit hitObject, 100, StaticUtilities.AttackLayers)) return;

        Vector3 normalizedHitPoint = hitObject.point.normalized;
        foreach (Monster monster in controlledMonsters)
        {
            monster.TryAttack(hitObject, normalizedHitPoint);
        }
    }

    public void MoveTo(Ray camToWorldRay)
    {
        Debug.DrawRay(camToWorldRay.origin, camToWorldRay.direction* 100, Color.blue,1);

        if (!Physics.Raycast(camToWorldRay, out RaycastHit hit, 100, StaticUtilities.MoveLayerMask))
            return;

        foreach (Monster monster in controlledMonsters)
        {
            monster.MoveToTarget(hit.point);
        }
        
    }
    
    
    public void SetRotationDirection(float direction)
    {
        _rotationDirection = direction;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection.x = direction.x;
        _moveDirection.z = direction.y;
    }
}
