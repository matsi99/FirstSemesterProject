using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyBehaviour : Enemy {

    private void attack ()
    {
        Player.GetComponent<Renderer>().enabled = false;
    }

}
