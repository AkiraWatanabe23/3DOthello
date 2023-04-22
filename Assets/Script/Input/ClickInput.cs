using UnityEngine;

public class ClickInput : MonoBehaviour
{
    [SerializeField] private Material[] _materials = new Material[2]; 

    private PlayerInput _input = default;
    private MeshRenderer _renderer = default;

    private void Start()
    {
        _input = GameObject.Find("System").GetComponent<PlayerInput>();
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = _materials[0];
    }

    private void OnMouseDown()
    {
        _input.BlackInput(gameObject.transform.position);
    }

    private void OnMouseEnter()
    {
        _renderer.material = _materials[1];
    }

    private void OnMouseExit()
    {
        _renderer.material = _materials[0];
    }
}
