using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public void ReturnToPoolHolder()
    {
        WorldItemPoolHolder.instance.PutItemsAwayToPool(this);
    }
}
