using UnityEngine;

public class EntityAnimationEvents : MonoBehaviour
{
    private Entity entity;


    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }
    public void DamageTargets() => entity.DamageTargets();
    private void DisableMovement() => entity.EnableMovement(false);
    private void EnableMovement() => entity.EnableMovement(true);
}
 