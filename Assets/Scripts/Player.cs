using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    private Rigidbody playerRigidbody;
    private MeshRenderer meshRenderer;

    public float speed = 3f;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();

        if (photonView.IsMine)
        {
            meshRenderer.material.color = Color.blue;
        }
        else
        {
            meshRenderer.material.color = Color.red;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        float v = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(0f, 0f, v) * speed * Time.deltaTime;
    }
}
