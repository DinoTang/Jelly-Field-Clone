using UnityEngine;

public class ExplosionEffectCtrl : PoolObj
{
    [Header("Explosion Effect")]
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private JellyColor color;

    public JellyColor Color => this.color;

    public override string GetName()
    {
        return $"ExplosionEffect_{this.color}";
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadParticleSystem();
    }

    protected void LoadParticleSystem()
    {
        if (this.particle != null) return;

        this.particle = GetComponentInChildren<ParticleSystem>();
        Debug.Log(transform.name + ": LoadParticleSystem");
    }

    public void Play()
    {
        if (this.particle == null) return;

        this.particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        this.particle.Play(true);
    }

    public void ResetEffect()
    {
        if (this.particle == null) return;

        this.particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        this.particle.Clear();
    }
}