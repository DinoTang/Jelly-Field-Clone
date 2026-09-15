using UnityEngine;

public class ExplosionEffectSpawn : Spawner<ExplosionEffectCtrl>
{
    public ExplosionEffectCtrl Spawn(JellyColor color, Vector3 position)
    {
        ExplosionEffectCtrl explosionEffectCtrl = this.GetExplosionEffectByColor(color);

        if (explosionEffectCtrl == null)
        {
            Debug.LogWarning($"No found explosionEffect for color: {color}");
            return null;
        }

        ExplosionEffectCtrl spawnedEffect = this.Spawn(explosionEffectCtrl, position);

        if (spawnedEffect != null)
            spawnedEffect.Play();

        return spawnedEffect;
    }

    private ExplosionEffectCtrl GetExplosionEffectByColor(JellyColor color)
    {
        foreach (ExplosionEffectCtrl explosionEffect in this.prefabs)
        {
            if (explosionEffect == null) continue;
            if (explosionEffect.Color != color) continue;

            return explosionEffect;
        }

        return null;
    }
}