using Harmonies.Enums;
using Unity.Netcode;
using UnityEngine;


namespace Harmonies.Selectors
{
    public class GameBlock : NetworkBehaviour
    {
        [field: SerializeField]
        public GameObject Prefab { get; private set; }

        [field: SerializeField]
        public BlockType Index { get; private set; }

        [ServerRpc(RequireOwnership = false)]
        public void DisableServerRpc() => GetComponent<NetworkObject>().Despawn();
    }
}
