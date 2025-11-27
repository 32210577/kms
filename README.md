
# Unity Meteor Sample

This contains minimal scripts for a "meteor" ranged weapon that drops meteors at random positions **inside the player's current camera view** at fixed intervals.

## Files
- `Assets/Scripts/MeteorSpawner.cs` — Spawns meteors at random visible points using viewport->raycast onto `Ground` layer.
- `Assets/Scripts/Meteor.cs` — Handles collision, AoE damage, and VFX/SFX hooks.
- `Assets/Scripts/IDamageable.cs` — Simple interface example for damage receivers.
- `.gitignore` — Unity-friendly ignore file.

## Quick Setup (Unity 2021+)
1. Create a `Ground` layer and assign it to your ground/terrain meshes.
2. Create a meteor prefab:
   - Add `Rigidbody` (Use Gravity ON), a `Collider`, and `Meteor.cs`.
   - Configure `hitMask` to the layers that should receive damage.
3. Add an empty GameObject in your scene and attach `MeteorSpawner.cs`.
   - Assign `Meteor Prefab` and set `groundMask` to `Ground`.
   - Optionally tweak interval, spawnHeight, and margins.
4. Hit Play.

## Git Quick Start
```bash
cd UnityMeteorSample
git init
git add .
git commit -m "Add meteor spawner and meteor scripts"
# Replace the URL with your own repo:
git remote add origin https://github.com/<YOUR-ID>/<YOUR-REPO>.git
git branch -M main
git push -u origin main
```

If you already have an existing repo, you can copy `Assets/Scripts/*` and update your `.gitignore` as needed.
