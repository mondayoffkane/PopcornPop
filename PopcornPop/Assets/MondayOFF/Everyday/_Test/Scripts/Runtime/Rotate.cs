using UnityEngine;

namespace MondayOFF {
    public class Rotate : MonoBehaviour {
        private void Update() {
            this.transform.Rotate(new Vector3(0f, 30f * Time.deltaTime, 0f));
        }
    }
}