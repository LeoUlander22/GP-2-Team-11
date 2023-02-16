using Team11.Health;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using FMODUnity;

public class VignetteIntensity : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float increaseSpeed = 1;
    [SerializeField] private bool useHealthRegen;

    private Vignette vignette;

    void Start()
    {
        PostProcessVolume volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out vignette);
    }

    void Update()
    {
        
        if (useHealthRegen)
        {
            vignette.intensity.value = GetIntensity();
            return;
        }
        
        if (playerHealth.IsBeingDamaged)
        {
            vignette.intensity.value = GetIntensity();
            
        }
        else
        {
            if(vignette.intensity.value >= 0) 
                vignette.intensity.value -= increaseSpeed * Time.deltaTime;
            
        }
        
    }

    float GetIntensity()
    {
        float t = playerHealth.CurrentHealth / playerHealth.MaxHealth;
        t = 1 - t;

        return t;
    }
    
    public void SetPlayerHealth(PlayerHealth playerHealth)
    {
        this.playerHealth = playerHealth;
        this.enabled = true;
    }
}
