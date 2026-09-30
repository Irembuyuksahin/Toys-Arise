using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private IntEvent onSwordContact;
    [SerializeField] private VoidEvent onWaveEnd;
    [SerializeField] private IntEvent onCoinGain;

    [SerializeField] private float enemyHealth = 10;
    [SerializeField] private float healthRegain;
    [SerializeField] private float swordDamage;
    [SerializeField] private float bananaDamage;

    [SerializeField] private GameObject poofEffect;
    [SerializeField] private GameObject coinEffect;

    public bool canTakeDamage = true;

    private bool isPushing = false;

    private List<Color[]> originalColors;
    private Renderer[] renderers;

    private Animator animator;
    private ParticleSystem particleSystem;
    private AudioManager audioManager;

    private void OnEnable()
    {
        onSwordContact.AddListener(SwordContact);
        onWaveEnd.AddListerner(OnDeathCall);
    }

    private void OnDisable()
    {
        onSwordContact.RemoveListener(SwordContact);
        onWaveEnd.RemoveListerner(OnDeathCall);
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        if (gameObject.CompareTag("Maymun"))
        {
            particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new List<Color[]>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            Color[] colors = new Color[materials.Length];

            for (int i = 0; i < materials.Length; i++)
            {
                colors[i] = materials[i].color;
            }

            originalColors.Add(colors);
        }

        if (gameObject.CompareTag("Asker"))
        {
            animator = GetComponentInChildren<Animator>();
        }
        else
        {
            animator = GetComponent<Animator>();
        }

        ShopManager shopManager = GameObject.Find("Shop Manager").GetComponent<ShopManager>();

        if (shopManager.shopItems[3, 4] > 0)
        {
            swordDamage += shopManager.shopItems[3, 4];
        }

        if (shopManager.shopItems[3, 5] > 0 && !gameObject.CompareTag("Maymun"))
        {
            bananaDamage += shopManager.shopItems[3, 5];
        }
    }

    private void SwordContact(int hash)
    {
        if (gameObject.GetHashCode() == hash && canTakeDamage)
        {
            enemyHealth -= swordDamage;

            HitReaction();
            StartCoroutine(ChangeColorRoutine());
            Death();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Muz") && canTakeDamage)
        {
            Destroy(collision.gameObject);
            audioManager.PlaySFX(audioManager.muzFiskirma);

            if (!gameObject.CompareTag("Maymun"))
            {
                TakeBananaDamage();
                HitReaction();
                StartCoroutine(ChangeColorRoutine());
                Death();
            }
            else
            {
                enemyHealth += healthRegain;
                particleSystem.gameObject.SetActive(true);
                particleSystem.Play();
            }
        }
    }

    private void TakeBananaDamage()
    {
        enemyHealth -= bananaDamage;
    }

    private void Death()
    {
        if (enemyHealth <= 0 && gameObject.CompareTag("Ayý"))
        {
            StartCoroutine(DeathCoroutine(300, 2.5f));
        }
        else if (enemyHealth <= 0 && gameObject.CompareTag("Topac"))
        {
            StartCoroutine(DeathCoroutine(50, 2f));
        }
        else if (enemyHealth <= 0 && gameObject.CompareTag("Maymun"))
        {
            StartCoroutine(DeathCoroutine(150, 3f));
        }
        else if (enemyHealth <= 0 && gameObject.CompareTag("Asker"))
        {
            StartCoroutine(DeathCoroutine(100, 2.5f));
        }
    }

    private IEnumerator DeathCoroutine(int coinAmount, float deathDuration)
    {
        canTakeDamage = false;
        animator.SetTrigger("isDeath");
        coinEffect.SetActive(true);
        onCoinGain.Raise(coinAmount);

        yield return new WaitForSeconds(deathDuration);

        Destroy(gameObject);
    }

    private void HitReaction()
    {
        if (gameObject.CompareTag("Ayý") || gameObject.CompareTag("Maymun") || gameObject.CompareTag("Asker"))
        {
            animator.SetTrigger("isHasarAldý");
        }

        if (!isPushing)
        {
            StartCoroutine(PushBackwardsCoroutine());
        }
    }

    private void OnDeathCall()
    {
        StartCoroutine(OnDeathCallCoroutine());
    }

    private IEnumerator OnDeathCallCoroutine()
    {
        if (canTakeDamage)
        {
            animator.SetTrigger("isDeath");
        }

        canTakeDamage = false;

        yield return new WaitForSeconds(3f);

        poofEffect.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

    private IEnumerator ChangeColorRoutine()
    {
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = Color.red;
            }
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials;
            Color[] colors = originalColors[i];

            for (int j = 0; j < materials.Length; j++)
            {
                materials[j].color = colors[j];
            }
        }
    }

    private IEnumerator PushBackwardsCoroutine()
    {
        isPushing = true;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition - transform.forward * 5;
        float elapsedTime = 0f;

        while (elapsedTime < 0.4f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.4f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isPushing = false;
    }
}