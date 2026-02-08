using UnityEngine;

public class PorteMa : MonoBehaviour
{
    private int currentIndex = 0;

    private void Update()
    {
        if (currentIndex >= transform.childCount) return;

        Transform current = transform.GetChild(currentIndex);

        OuverturePorte op = current.GetComponent<OuverturePorte>();
        if (op == null) return;

        if (!op.opened) return;

        op.enabled = false;

        int nextIndex = currentIndex + 1;
        if (nextIndex < transform.childCount)
        {
            Transform next = transform.GetChild(nextIndex);
            next.gameObject.SetActive(true);
        }

        currentIndex = nextIndex;
    }
}
