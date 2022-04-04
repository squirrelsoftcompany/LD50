// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-03

using System;
using System.Collections;
using Environment;
using UnityEngine;

namespace Player.ResourcesUsable {
public class Canadair : Resource {
    private ParticleSystem _particle;
    [SerializeField] private float distanceFlyingBefore = 60;
    [SerializeField] private float distanceFlyingAfter = 30;
    [SerializeField] private int maxFlyingBeats = 20;
    [SerializeField] private int maxManeuverBeats = 3;

    [Tooltip("Angle for the arc of a circle after the effect (in radian)")] [SerializeField]
    private float fullManeuverAngle = Mathf.PI / 2;

    private float eulerAngle() => fullManeuverAngle * Mathf.Rad2Deg;

    [SerializeField] private float radiusManeuverBack = 1f;

    private float spf => 1f / fps;

    protected override void Awake() {
        base.Awake();
        _particle = GetComponentInChildren<ParticleSystem>();
    }

    protected override void applyEffect() {
        foreach (var vector2Int in neighbours) {
            ref var neighbor = ref World.Inst[vector2Int];
            if (neighbor.m_type == Environment.Tile.TileType.eNone) continue;
            neighbor.Intensity = (int)Math.Clamp(neighbor.Intensity - Characteristics.efficiency,
                World.minFireIntensity, World.maxFireIntensity);
        }
    }

    protected override IEnumerator spawnAnimation() {
        transform.localPosition = Vector3.right * distanceFlyingBefore;
        // transform.Translate(Vector3.left * distanceFlyingBefore);

        var lineBeats = Characteristics.spawnWait - 0.5f; // account for de-synchronization of beats
        var lineSec = 60f * lineBeats / GameManagerCool.Inst.bpm;
        var xIncrements = distanceFlyingBefore * spf / lineSec;
        while (transform.localPosition.x > 0) {
            transform.localPosition += Vector3.left * xIncrements;
            yield return new WaitForSeconds(spf);
        }
    }

    protected override IEnumerator showInCooldown() {
        var emissionModule = _particle.emission;
        emissionModule.enabled = false;

        // maneuver time (draw a quarter of a circle in the air)
        var maneuverBeats = Math.Min(maxManeuverBeats, Characteristics.cooldown);
        var maneuverSec = 60f * maneuverBeats / GameManagerCool.Inst.bpm;

        var thetaIncrements = fullManeuverAngle * spf / maneuverSec;
        var angle = new Vector3(0f, 0f, 0f);
        var angleIncrement = -eulerAngle() * spf / maneuverSec;
        var theta = 0f;
        while (theta < fullManeuverAngle) {
            theta += thetaIncrements;
            var newPos = new Vector3(-radiusManeuverBack * Mathf.Sin(theta),
                transform.localPosition.y, radiusManeuverBack * (Mathf.Cos(theta) - 1));
            angle.y += angleIncrement;
            transform.localEulerAngles = angle;
            transform.localPosition = newPos;
            yield return new WaitForSeconds(spf);
        }

        // now for the last line
        if (maneuverBeats >= Characteristics.cooldown) yield return null;

        var lineBeats = Math.Min(Characteristics.cooldown - maneuverBeats,
            Math.Max(0, maxFlyingBeats - maneuverBeats));
        var lineSec = 60f * lineBeats / GameManagerCool.Inst.bpm;
        var zIncrements = distanceFlyingAfter * spf / lineSec;
        while (transform.localPosition.z < distanceFlyingAfter) {
            transform.localPosition += Vector3.back * zIncrements;
            yield return new WaitForSeconds(spf);
        }
    }

    protected override IEnumerator showActive() {
        var emissionModule = _particle.emission;
        emissionModule.enabled = true;
        yield return null;
    }
}
}