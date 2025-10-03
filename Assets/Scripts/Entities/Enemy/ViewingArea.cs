using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ViewingArea : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            //if (npcMovement.areaCollider.bounds.Contains(collider.transform.position))
            //{
            //    npcMovement.target = collider.transform;
            //    npcMovement.isFollowing = true;
            //}
        }
    }

}
