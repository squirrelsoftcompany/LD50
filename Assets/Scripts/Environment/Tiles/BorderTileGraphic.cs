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

        protected override void InstantiateTile(Tile.TileType tileType)
        {
            base.InstantiateTile(tileType == Tile.TileType.eRoad ? Tile.TileType.ePlain : tileType);
        }

        public override void UpdateHumidity()
        {
            // do nothing
        }
    }
}
