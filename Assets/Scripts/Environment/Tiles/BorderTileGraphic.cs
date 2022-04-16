using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class BorderTileGraphic : TileGraphic
    {
        [Header("Front line")]
        public GameObject frontLineRepresentation;

        public override void UpdateFire()
        {
            base.UpdateFire();

            frontLineRepresentation?.SetActive(World.Inst.IsOnFireFrontline(m_position));
        }

        public override void UpdateHumidity()
        {
            // do nothing
        }
    }
}
