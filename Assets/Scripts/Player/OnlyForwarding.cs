using System;
using UnityEngine;

namespace Player {
public class OnlyForwarding : MonoBehaviour {
    private IMortal _mortal;
    private Civilian _civilian;
    private void Awake() {
        _mortal = GetComponentInParent<IMortal>();
        _civilian = GetComponentInParent<Civilian>();
    }

    public void doDie() {
        _mortal.doDie();
    }

    public void onSaveAnimEnded() {
        if (_civilian != null) {
            _civilian.onSaveAnimEnded();
        }
    }
}
}
