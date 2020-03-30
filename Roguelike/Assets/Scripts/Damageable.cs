using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damageable {

    void TakeHit(float damage, RaycastHit hit);

}
