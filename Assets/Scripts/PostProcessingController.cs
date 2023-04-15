using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingController : MonoBehaviour
{
    public static PostProcessingController instance;

    private PostProcessVolume volume;

    private MotionBlur _motionBlur;
    private Bloom _bloom;
    private LensDistortion _lensDistortion;
    private Vignette _vignette;

    public bool motionBlur = true;
    public bool bloom = true;
    public bool lensDistortion = true;
    public bool vignette = true;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out _motionBlur);
        volume.profile.TryGetSettings(out _bloom);
        volume.profile.TryGetSettings(out _lensDistortion);
        volume.profile.TryGetSettings(out _vignette);
        SetEffects();
    }

    public void SetEffects()
    {
        _motionBlur.active = motionBlur;
        _bloom.active = bloom;
        _lensDistortion.active = lensDistortion;
        _vignette.active = vignette;
    }
}
