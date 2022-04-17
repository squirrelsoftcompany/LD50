using UnityEngine;

namespace UI {
public class CivilianUI : MonoBehaviour {

    public void showCivilianAppeared() {
        Time.timeScale = 0f;
    }

    public void hideCivilianAppeared() {
        Time.timeScale = GameManagerCool.Inst.NormalTimeScale;
    }
}
}
