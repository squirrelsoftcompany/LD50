// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-03

using System;
using System.Collections;
using Environment;
using UnityEngine;

namespace Player.ResourcesUsable {
public class CanadairDelay : Resource {
    private ParticleSystem _particle;
    [SerializeField] private float distanceFlyingBefore = 60;
    [SerializeField] private float distanceFlyingAfter = 30;
    [SerializeField] private int maxFlyingTime = 20;
    [SerializeField] private float fps = 24f;

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
        transform.localPosition = Vector3.left * distanceFlyingBefore;
        // transform.Translate(Vector3.left * distanceFlyingBefore);
        var spf = 1f / fps;
        var speed = distanceFlyingBefore /
                    (Math.Min(Characteristics.spawnWait, maxFlyingTime) * fps);

        while (transform.localPosition.x < 0) {
            transform.localPosition += Vector3.right * speed;
            yield return new WaitForSeconds(spf);
        }
    }

    protected override IEnumerator showInCooldown() {
        var emissionModule = _particle.emission;
        emissionModule.enabled = false;
        var spf = 1f / fps;
        var speed = distanceFlyingAfter / (Math.Min(Characteristics.cooldown, maxFlyingTime) * fps);
        while (transform.localPosition.x < distanceFlyingAfter) {
            transform.localPosition+= Vector3.right * speed;
            // transform.Translate(Vector3.right * speed);
            yield return new WaitForSeconds(spf);
        }

        Debug.Log("finished");
    }

    protected override IEnumerator showActive() {
        var emissionModule = _particle.emission;
        emissionModule.enabled = true;
        yield return null;
    }
}
}