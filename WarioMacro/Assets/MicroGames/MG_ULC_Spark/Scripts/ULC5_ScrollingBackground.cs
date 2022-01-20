using UnityEngine;

public class ULC5_ScrollingBackground : MonoBehaviour {

    private Material mat;
    [SerializeField] private float scrollSpeed;

    void Awake() {
        mat = GetComponent<MeshRenderer>().material;
    }
    
    void Update() {
        Vector2 offset = mat.mainTextureOffset;
        offset.y += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}
