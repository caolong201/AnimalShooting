using System.Collections.Generic;
using UnityEngine;

namespace IE.RSB
{
    public class Enemy : MonoBehaviour
    {
        public class TransformStamp
        {
            public Transform m_transform;
            public Vector3 m_position;
            public Quaternion m_rotation;
        }

        [SerializeField] private Animator m_animator = null;
        [SerializeField] private int animalNo = 0;

        // Private class members.
        private CapsuleCollider[] m_ragdollBodies;
        private BulletTimeTarget[] m_btTargets;
        private List<TransformStamp> m_transformStamps = new List<TransformStamp>();
        private bool m_isDead = false;

        private GameManager m_gameManager = null;
        
        //デフォルトは0で待機モーションのみ
        public int moveAnimation = 0;
        private bool moveFlg = false;
        private float speed =0.2f;
        public bool isDead => m_isDead;
        void Awake()
        {
            m_gameManager = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();
            m_ragdollBodies = GetComponentsInChildren<CapsuleCollider>();
            m_btTargets = GetComponentsInChildren<BulletTimeTarget>();

            // Saves the current pose, meaning all the children transforms' position & rotation will be saved in a list of TransformStamps.
            SaveCurrentPose();

            // Disable bodies in awake.
            //RagdollBodiesIsKinematic(true);
        }

        private void OnEnable()
        {
            SniperAndBallisticsSystem.EAnyHit += OnAnyHit;
        }



        private void OnDisable()
        {
            SniperAndBallisticsSystem.EAnyHit -= OnAnyHit;

            // Make sure restore pose invoke is not running & restore pose immediately.
            CancelInvoke("RestorePose");
            RestorePose();
        }

        private void OnAnyHit(BulletPoint point)
        {
            if (m_isDead) return;

            // Check if the bullet hit any of the ragdoll bodies.
            for (int i = 0; i < m_ragdollBodies.Length; i++)
            {
                // If yes, enable all ragdolls and set dead flag.
                if (point.m_hitTransform == m_ragdollBodies[i].transform)
                {
                    //RagdollBodiesIsKinematic(false);
                    //Debug.Log("4まできた");
                    m_animator.SetTrigger("Death");
                    if(moveFlg) moveFlg = false;
                    //Debug.Log("5まできた");
                    // Some of our rigidbody parts are bullet time targets, e.g when bullet hits head or torso bullet time might be triggered.
                    // But we don't want to trigger bullet time if the enemy is already dead, so deactivate those targets.
                    BulletTimeTargetsActivation(false);


                    // 👉 Xác định trúng đầu hay thường
                    bool isHeadshot = point.m_hitTransform.name.ToLower().Contains("head");

                    // 👉 Gọi UIManager để hiển thị hiệu ứng
                    UIManager uiManager = FindObjectOfType<UIManager>();
                    if (uiManager != null)
                    {
                        uiManager.ShowHitIndicator(isHeadshot);
                    }



                    // Animator & flag.
                    //m_animator.enabled = false;
                    m_isDead = true;

                    if(m_gameManager){
                        Debug.Log("Death："+m_isDead);
                        m_gameManager.EnemyDown(animalNo);
                    }

                    // Restore our initial pose after 4 seconds.
                    //Invoke("RestorePose", 4f);

                    break;
                }
            }
        }

        void Update(){
            //移動させる
            if(moveFlg){
                transform.position += transform.forward * speed * Time.deltaTime;
                
            }
        }

        void RestorePose()
        {
            Debug.Log("ここ入っている");
            // Make bodies kinematic & enable bullet time target components in the body parts again so that we can trigger bullet time again.
            //RagdollBodiesIsKinematic(true);
            BulletTimeTargetsActivation(true);

            for (int i = 0; i < m_transformStamps.Count; i++)
            {
                m_transformStamps[i].m_transform.localPosition = m_transformStamps[i].m_position;
                m_transformStamps[i].m_transform.localRotation = m_transformStamps[i].m_rotation;
            }

            m_animator.enabled = true;
            m_isDead = false;
        }


        private void SaveCurrentPose()
        {
            m_transformStamps.Clear();
            Transform[] allTransforms = GetComponentsInChildren<Transform>();

            for (int i = 0; i < allTransforms.Length; i++)
            {
                TransformStamp stamp = new TransformStamp();
                stamp.m_transform = allTransforms[i];
                stamp.m_position = allTransforms[i].localPosition;
                stamp.m_rotation = allTransforms[i].localRotation;
                m_transformStamps.Add(stamp);
            }

        }

        private void BulletTimeTargetsActivation(bool activate)
        {
            for (int i = 0; i < m_btTargets.Length; i++)
                m_btTargets[i].SetActivation(activate);
        }


        public void DoMoveAnimal(int animationNo){
            if(animationNo > 0){
                switch(animationNo){
                    case 1:
                        m_animator.SetInteger("AnimationNo",animationNo);
                        break;
                    case 2:
                        //m_animator.SetInteger("AnimationNo",animationNo);
                        //移動するフラグ
                        Debug.Log("通った:"+this.transform);
                        m_animator.SetInteger("AnimationNo",animationNo);
                        moveFlg = true;
                        break;
                    default :
                        break;
                }
            }
        }
    }
}