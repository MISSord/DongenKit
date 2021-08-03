using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun 
{
    void Init();
    void FadeGun();
    void UpdateGunPosture();
    void Shoot();

}
