using UnityEngine;

namespace UI {
public class CivilianUI : MonoBehaviour {
    private float _formerTimeScale = 1f;

    public void showCivilianAppeared() {
        _formerTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void hideCivilianAppeared() {
        Time.timeScale = _formerTimeScale;
    }
}
}
