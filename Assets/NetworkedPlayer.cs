using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class NetworkedPlayer : NetworkBehaviour
{
    [Header("Change bodymesh shadowcast")]
    [SerializeField] private SkinnedMeshRenderer[] _shadowCasterMeshRenderer;
    [Header("Change Camera Culling for Other players heads to render")]
    [SerializeField] private string _headCullLayerMask;
    [SerializeField] private string _headRenderLayerMask;
    [SerializeField] private GameObject[] _headObject;

    [Header("Objects to disable on other")]
    [SerializeField] private Camera _cameraToDisable;
    [SerializeField] private MonoBehaviour[] _scriptsToDisable;
    [SerializeField] private Rigidbody[] _rigidbodiesToDisable;
    [SerializeField] private GameObject[] _objectsToDisable;

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
        Debug.Log($"Prev {prevOwner.ClientId} ownerid {base.OwnerId}");
        DisableCamera(base.IsOwner);
        DisableRigidbodies(base.IsOwner);
        DisableObjects(base.IsOwner);
        DisableScripts(base.IsOwner);

        var layerToFind = base.IsOwner ? _headCullLayerMask : _headRenderLayerMask;
        foreach (var gameobj in _headObject)
        {
            gameobj.layer = LayerMask.NameToLayer(layerToFind);
        }
        
        //shadows
        var castShadows = base.IsOwner ? UnityEngine.Rendering.ShadowCastingMode.Off : UnityEngine.Rendering.ShadowCastingMode.On;
        foreach(var skinnedRenderer in _shadowCasterMeshRenderer)
        {
            skinnedRenderer.shadowCastingMode = castShadows;
        }
    }

    void DisableCamera(bool isActive = false)
    {
        _cameraToDisable.enabled = isActive;
    }

    void DisableRigidbodies(bool isActive = false)
    {
        foreach (var obj in _rigidbodiesToDisable)
        {
            obj.isKinematic = !isActive;
        }
    }

    void DisableScripts(bool isActive = false)
    {
        foreach (var obj in _scriptsToDisable)
        {
            obj.enabled = isActive;
        }
    }

    void DisableObjects(bool isActive = false)
    {
        foreach (var obj in _objectsToDisable)
        {
            obj.SetActive(isActive);
        }
    }
}
