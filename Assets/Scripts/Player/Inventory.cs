using System;
using System.Collections.Generic;
using Player.ResourcesUsable;
using ScriptableObjects;
using UnityEngine;

namespace Player {
public class Inventory : MonoBehaviour, ITick {
    private Dictionary<ResourceCharacteristics, List<Resource>> _availableResources;
    [SerializeField] private List<ResourceCharacteristics> allResourceTypes;

    private List<Resource> _resourceUsables;

    // Start is called before the first frame update
    private void Start() {
        _resourceUsables = new List<Resource>();
        _availableResources = new Dictionary<ResourceCharacteristics, List<Resource>>();
        foreach (var resourceType in allResourceTypes) {
            _availableResources[resourceType] = new List<Resource>();
        }
    }

    // Update is called once per frame
    void Update() { }

    public void callToMap(ResourceCharacteristics resourceType, Transform position) {
        var newResource = Instantiate(resourceType.prefab, transform);
        
        
    }


    public void tick() {
        foreach (var resource in _resourceUsables) {
            resource.tick();
            switch (resource.State) {
                case ResourceState.Idle:
                    break;
                case ResourceState.Spawning:

                    break;
                case ResourceState.Active:
                    break;
                case ResourceState.Cooldown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        throw new System.NotImplementedException();
    }
}
}