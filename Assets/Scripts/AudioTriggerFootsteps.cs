using UnityEngine;

public class AudioTriggerFootsteps : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip impactClip;
    [SerializeField] private AudioClip backgroundMusicClip;

    [Header("Audio Settings")]
    [SerializeField] private float footstepVolume = 0.3f;
    [SerializeField] private float jumpVolume = 0.5f;
    [SerializeField] private float impactVolume = 0.6f;
    [SerializeField] private float musicVolume = 0.4f;
    [SerializeField] private float footstepCooldown = 0.4f;

    private AudioSource audioSource;
    private AudioSource musicSource;
    private PlayerMovement playerMovement;
    private Rigidbody rb;
    private float footstepTimer = 0f;
    private bool wasGrounded = false;
    private Vector3 lastVelocity;

    void Start()
    {
        // Get or create audio source for SFX
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Create a second audio source for background music
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;

        // Get player components
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        lastVelocity = rb.linearVelocity;

        // Play background music
        PlayBackgroundMusic();
    }

    void Update()
    {
        HandleFootsteps();
        HandleJump();
        lastVelocity = rb.linearVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Play impact sound on collision
        PlayImpactSound(collision.relativeVelocity.magnitude);
    }

    void HandleFootsteps()
    {
        // Decrement footstep timer
        footstepTimer -= Time.deltaTime;

        // Check if player is moving on ground
        bool isMoving = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude > 0.5f;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, LayerMask.GetMask("Default"));

        if (isMoving && isGrounded && footstepTimer <= 0)
        {
            PlayFootstepSound();
            footstepTimer = footstepCooldown;
        }
    }

    void HandleJump()
    {
        // Check if player just jumped (velocity increased upward)
        if (rb.linearVelocity.y > lastVelocity.y && rb.linearVelocity.y > 1f)
        {
            PlayJumpSound();
        }
    }

    void PlayFootstepSound()
    {
        if (footstepClip != null)
        {
            audioSource.PlayOneShot(footstepClip, footstepVolume);
        }
    }

    void PlayJumpSound()
    {
        if (jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip, jumpVolume);
        }
    }

    void PlayImpactSound(float velocity)
    {
        if (impactClip != null && velocity > 2f)
        {
            // Scale volume based on impact force
            float volume = Mathf.Clamp01(impactVolume * (velocity / 10f));
            audioSource.PlayOneShot(impactClip, volume);
        }
    }

    void PlayBackgroundMusic()
    {
        if (backgroundMusicClip != null && !musicSource.isPlaying)
        {
            musicSource.clip = backgroundMusicClip;
            musicSource.Play();
        }
    }

    // Public methods to control music
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }
}