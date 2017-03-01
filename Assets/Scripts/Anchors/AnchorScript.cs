using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorScript : AbstractAnchorScript
{
    public override void Respawn()
    {
        GameObject go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Anchors/Anchor"), this.transform.parent);
        go.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }
}
