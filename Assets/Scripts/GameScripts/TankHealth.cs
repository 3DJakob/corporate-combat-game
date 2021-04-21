using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;          
    public Slider m_Slider;                        

    public Image m_FillImage;                      
    public Color m_FullHealthColor = Color.green;  
    public Color m_ZeroHealthColor = Color.red;    
    //public GameObject m_ExplosionPrefab;
    
    
    private AudioSource m_ExplosionAudio;          
    private ParticleSystem m_ExplosionParticles;   

    [SerializeField]
    private float m_CurrentHealth;  
    private bool m_Dead;    
    public int team;

    void Start(){
        team = GetComponent<TankNav>().team;
    }

    private void Awake()
    {
        //m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        //m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        //m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }
    

    public bool TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        
        PhotonView PV = GetComponent<PhotonView>();
        PV.RPC("RPC_UpdateHealth", RpcTarget.All, amount);
        //m_CurrentHealth -= amount;
        //Debug.Log(this.name + m_CurrentHealth);
        //SetHealthUI();
        if(m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
            return true;
        }

        return false;
    }

    [PunRPC]
    void RPC_UpdateHealth(float amount) {
       
        m_CurrentHealth  -= amount;
        Debug.Log("I am taking damage!");
        
        if(m_Slider != null)
            SetHealthUI();
    }




    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        m_Slider.value = m_CurrentHealth;

        Debug.Log(this.name + m_Slider.value);

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;
        //m_ExplosionParticles.transform.position = transform.position;
        //m_ExplosionParticles.gameObject.SetActive(true);
        if (gameObject.tag == "Finish")
        {
            //Send event to photonPlayer and attach this.gameObject.layer
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Calling event");
                Debug.Log("Layer is " + LayerMask.LayerToName(this.gameObject.layer));
                byte eventId = 2;
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(eventId, LayerMask.LayerToName(this.gameObject.layer), raiseEventOptions, SendOptions.SendReliable);
            }
        }
        //m_ExplosionParticles.Play();

        //m_ExplosionAudio.Play();

        Debug.Log("BOOOOOOM");
        PhotonNetwork.Destroy(gameObject);
    }
}