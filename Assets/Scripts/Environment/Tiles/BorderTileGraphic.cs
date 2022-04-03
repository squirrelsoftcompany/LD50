using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class BorderTileGraphic : TileGraphic
    {
        public GameObject frontLineRepresentation;

        public override void UpdateFire()
        {
            base.UpdateFire();

            frontLineRepresentation?.SetActive(World.Inst.IsOnFireFrontline(m_position));
        }
    }
}
