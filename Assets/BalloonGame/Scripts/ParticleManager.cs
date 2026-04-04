using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    private Material particleMaterial;

    private void Awake()
    {
        Instance = this;
        particleMaterial = new Material(Shader.Find("Particles/Standard Unlit"));
        particleMaterial.SetFloat("_Mode", 0);
        particleMaterial.SetInt("_ColorMode", 0);
    }

    public void PlayShieldHit(Vector3 position, Color color)
    {
        var go = new GameObject("ShieldHitFX");
        go.transform.position = position;
        var ps = go.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration = 0.3f;
        main.startLifetime = 0.4f;
        main.startSpeed = new ParticleSystem.MinMaxCurve(3f, 6f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.15f);
        main.startColor = color;
        main.maxParticles = 15;
        main.loop = false;
        main.playOnAwake = true;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.gravityModifier = 0.5f;
        main.stopAction = ParticleSystemStopAction.Destroy;

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 12) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.1f;

        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        var gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(color, 0.3f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = gradient;

        var sizeOverLifetime = ps.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0, 1, 1, 0));

        var renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = particleMaterial;
        renderer.sortingOrder = 15;

        ps.Play();
    }

    public void PlayBalloonPop(Vector3 position, Color color)
    {
        var go = new GameObject("BalloonPopFX");
        go.transform.position = position;
        var ps = go.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration = 0.5f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.4f, 0.8f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(4f, 8f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.25f);
        main.startColor = color;
        main.maxParticles = 30;
        main.loop = false;
        main.playOnAwake = true;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.gravityModifier = 1f;
        main.stopAction = ParticleSystemStopAction.Destroy;

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 25) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.2f;

        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        var gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color * 0.5f, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = gradient;

        var sizeOverLifetime = ps.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0, 1, 1, 0.2f));

        var renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = particleMaterial;
        renderer.sortingOrder = 20;

        ps.Play();
    }

    public void PlayScoreMilestone(Vector3 position)
    {
        var go = new GameObject("MilestoneFX");
        go.transform.position = position;
        var ps = go.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration = 0.3f;
        main.startLifetime = 0.5f;
        main.startSpeed = new ParticleSystem.MinMaxCurve(1f, 3f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.03f, 0.08f);
        main.startColor = new Color(1f, 0.9f, 0.3f);
        main.maxParticles = 20;
        main.loop = false;
        main.playOnAwake = true;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.gravityModifier = -0.3f;
        main.stopAction = ParticleSystemStopAction.Destroy;

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 15) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.5f;

        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        var gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(new Color(1f, 0.9f, 0.3f), 0f), new GradientColorKey(new Color(1f, 0.6f, 0.1f), 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = gradient;

        var renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = particleMaterial;
        renderer.sortingOrder = 25;

        ps.Play();
    }
}
