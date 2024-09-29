using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField] private GameObject chainLinkPrefab;
    [SerializeField] private int chainLength;
    [SerializeField] private float linkOffset = -0.35f;
    [SerializeField] private List<GameObject> chain;
    private GameObject anchor;
    private Transform previousLink;

    private void Start()
    {
        anchor = transform.GetChild(0).gameObject;
        CreateChain();
    }

    private void CreateChain()
    {
        previousLink = anchor.transform;

        for (int i = 0; i < chainLength; i++) {
            Vector3 newPos = previousLink.position;
            if (i != 0) newPos.y += linkOffset;
            newPos.z -= ((i + 1)/ 100);

            GameObject newLink = Instantiate(chainLinkPrefab, newPos, Quaternion.identity, transform);
            HingeJoint2D hinge = newLink.GetComponent<HingeJoint2D>();
            hinge.connectedBody = previousLink.GetComponent<Rigidbody2D>();

            previousLink = newLink.transform;
            chain.Add(newLink);
        }

        // Need to set this to false to avoid some jank
        chain[0].GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
    }
}
