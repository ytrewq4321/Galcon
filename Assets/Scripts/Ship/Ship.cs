using UnityEngine;
using UnityEngine.AI;

public class Ship : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;

    private Owner owner;
    private Transform target;
    private bool isShipLaunched;

    private void Start()
    {
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.speed = speed;
    }

    public void SetOwner(Owner owner)
    {
        this.owner = owner;
        SetColor(owner);
    }

    private void Update()
    {
        if (!isShipLaunched && navMeshAgent.hasPath)
            isShipLaunched = true;

        if (isShipLaunched && !navMeshAgent.hasPath)
        {
            isShipLaunched = false;
            ShipPool.Instance.ShipObjectPool.Release(this);
            GlobalAction.ShipLanded.Invoke(owner, target.transform);
        } 
    }

    private void SetColor(Owner owner)
    {
        switch (owner)
        {
            case Owner.Player:
                spriteRenderer.color = Color.green;
                break;
            case Owner.AI:
                spriteRenderer.color = Color.red;
                break;
            default:
                spriteRenderer.color = Color.white;
                break;
        }
    }

    public void LaunchShip(Transform target)
    {
        this.target = target;
        Vector3 direction = (transform.position - target.position).normalized;
        Vector3 closestPoint = target.position + direction * target.localScale.x/2;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+90));

        navMeshAgent.SetDestination(closestPoint);
    }
}
