using System;
using UnityEngine;

namespace Player {
public class OnlyForwarding : MonoBehaviour {
    private IMortal _mortal;
    private void Awake() {
        _mortal = GetComponentInParent<IMortal>();
    }

    public void doDie() {
        _mortal.doDie();
    }
}
}
