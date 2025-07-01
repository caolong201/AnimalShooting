using UnityEngine;

namespace IE.RSB
{
    public class DemoMobileController : MonoBehaviour
    {
        //  //回転速度
        // //[Range(0f,1f)] public float rotationSpeed = 0.1f;
        // //縦方向の角度(上側）
        // [Range(-10f, 50f)] public float max_rotation_z = 13f;
        // //縦方向の角度(下側)
        // [Range(-20f, 50f)] public float min_rotation_z = -20f;
        // //左右方向の最大角度(左右対称のため最大のみ)
        // [Range(0f, 180f)] public float max_rotation_y = 120f;
        // [Range(0f, 180f)] public float min_rotation_y = 60f;

        // public Vector2 rotationSpeed = new Vector2(0.1f, 0.1f);
        // public bool reverse;

        // private Vector2 lastMousePosition;
        // private Vector3 newAngle = Vector3.zero;

        // bool isMouseDown = false;
        // bool isMouseDownCheck = false;

        // // Public Access
        // public static Vector2 LookInput { get { return s_lookInput; } }
        // private static Vector2 s_lookInput = Vector2.zero;
        
        // private void Awake()
        // {
        //     // GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        //     // if(objects.Length > 1)
        //     // {
        //     //     Destroy(gameObject);
        //     // }else{
        //     //     DontDestroyOnLoad(gameObject);
        //     // }
        // }

        // void Start()
        // {

        // }

        // void Update()
        // { 

        //     if (Input.GetMouseButton(0)&& isMouseDown)
        //     {  
        //         if(isMouseDownCheck){
        //             if (!reverse)
        //             {   
        //                 s_lookInput.x -= (lastMousePosition.x - Input.mousePosition.x) * rotationSpeed.x;
        //                 s_lookInput.y -= (Input.mousePosition.y - lastMousePosition.y) * rotationSpeed.y;
        //             }else{
        //                 s_lookInput.x -= (Input.mousePosition.x - lastMousePosition.x) * rotationSpeed.y;
        //                 s_lookInput.y -= (lastMousePosition.y - Input.mousePosition.y) * rotationSpeed.x;
        //             }        
        //             s_lookInput.y = Mathf.Clamp(newAngle.y,min_rotation_y,max_rotation_y);
        //             s_lookInput.x = Mathf.Clamp(newAngle.z,min_rotation_z,max_rotation_z);
        //         }
        //         //this.transform.localEulerAngles = newAngle;
        //         lastMousePosition = Input.mousePosition;
        //         isMouseDownCheck = true;

        //         Debug.Log("テスト；"+s_lookInput);
        //         Debug.Log("x:"+s_lookInput.x+"y:"+s_lookInput.y);
        //     }

        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         // マウスボタンが押されたらフラグをセット
        //         isMouseDownCheck = true;
        //         isMouseDown = true;
        //         // newAngle = this.transform.localEulerAngles;
        //         lastMousePosition = Input.mousePosition;
        //     }

        //     if (Input.GetMouseButtonUp(0) && isMouseDown)
        //     {
        //         // マウスボタンが離れたら発射
        //         isMouseDown = false;
        //         isMouseDownCheck = false;
        //         // CameraShaker();
        //         // CannonFiring();
        //     }

        // }
        // Exposed
        [SerializeField] private float m_joystickResetSpeed = 500.0f;
        [SerializeField] private float m_lookInputMultiplier = 0.25f;
        [SerializeField] private RectTransform m_lookJoystickBase = null;
        [SerializeField] private RectTransform m_lookJoystickHandle = null;
        [SerializeField] private RectTransform m_moveJoystickHandle = null;
        [SerializeField] private PlayerWeaponController m_weaponController = null;
        [SerializeField] private DynamicScopeSystem m_dynamicScopeSystem = null;
        [SerializeField] private GameObject[] m_controlsToEnableOnlyInAim = new GameObject[] { };

        // Public Access
        public static Vector2 LookInput { get { return s_lookInput; } }
        public static Vector2 MoveInput { get { return s_moveInput; } }

        // Private members
        private float m_joystickMaxRadius = 0.0f;
        private bool m_isControlsInAimEnabled = false;
        private DemoMobileJoystick m_lookJoystick = null;
        private DemoMobileJoystick m_moveJoystick = null;
        private static Vector2 s_moveInput = Vector2.zero;
        private static Vector2 s_lookInput = Vector2.zero;
        private Vector2 lookPosition = Vector2.zero;

        private void Awake()
        {
            m_joystickMaxRadius = m_lookJoystickBase.sizeDelta.x / 2.0f - m_lookJoystickHandle.sizeDelta.x / 2.0f - 2;
            m_lookJoystick = m_lookJoystickHandle.GetComponent<DemoMobileJoystick>();
            m_moveJoystick = m_moveJoystickHandle.GetComponent<DemoMobileJoystick>();

            AimControlsActivation(false);

            // Disable if we are not on mobile.
#if !(!UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS))
            //this.enabled = false;
#else
            Input.simulateMouseWithTouches = false;
#endif
        }

        public void PointerDown_AimButton()
        {
            m_weaponController.AimInput(!m_isControlsInAimEnabled);

            // Enable aim controls (zoom in&out, zero distance up&down) if aimed in.
            m_isControlsInAimEnabled = !m_isControlsInAimEnabled;
            AimControlsActivation(m_isControlsInAimEnabled);
        }

        public void PointerDown_FireButton()
        {
            m_weaponController.FireInput();
        }

        public void PointerDown_ReloadButton()
        {
            m_weaponController.ReloadInput();
        }

        public void PointerDown_ZoomIn()
        {
            if (m_dynamicScopeSystem)
                m_dynamicScopeSystem.ZoomIn();
        }

        public void PointerDown_ZoomOut()
        {
            if (m_dynamicScopeSystem)
                m_dynamicScopeSystem.ZoomOut();
        }

        public void PointerDown_ZeroDistanceUpButton()
        {
            SniperAndBallisticsSystem.instance.CycleZeroDistanceUp();
        }

        public void PointerDown_ZeroDistanceDownButton()
        {
            SniperAndBallisticsSystem.instance.CycleZeroDistanceDown();
        }

        private void Update()
        {

        if (Input.GetMouseButton(0)){
            //Vector2 lookPosition = m_lookJoystick.GetEventPosition();
            //Vector2 movePosition = m_moveJoystick.GetEventPosition();
            //lookPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (lookPosition != Vector2.zero)
            {
                Vector2 delta = new Vector2(lookPosition.x, lookPosition.y) - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                delta = Vector2.ClampMagnitude(delta, m_joystickMaxRadius);
                m_lookJoystickHandle.localPosition = delta;
                s_lookInput = new Vector2(delta.x / m_joystickMaxRadius, delta.y / m_joystickMaxRadius) * m_lookInputMultiplier;
                s_lookInput = Vector2.zero;
            }
            else
            {
                m_lookJoystickHandle.localPosition = Vector3.MoveTowards(m_lookJoystickHandle.localPosition, Vector3.zero, Time.unscaledDeltaTime * m_joystickResetSpeed);
                s_lookInput = Vector2.zero;
            }
        }
            if (Input.GetMouseButtonDown(0))
            {
                // マウスボタンが押されたらフラグをセット
                // isMouseDownCheck = true;
                // isMouseDown = true;
                // newAngle = this.transform.localEulerAngles;
                lookPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }

            if (Input.GetMouseButtonUp(0))
            {
                // マウスボタンが離れたら発射
                // isMouseDown = false;
                // isMouseDownCheck = false;
                lookPosition = Vector2.zero;
            }

            // // Movement joystick
            // if (movePosition != Vector2.zero)
            // {
            //     Vector2 delta = new Vector2(movePosition.x, movePosition.y) - m_moveJoystick.GetDragBeginPosition();
            //     delta = Vector2.ClampMagnitude(delta, m_joystickMaxRadius);
            //     m_moveJoystickHandle.localPosition = delta;
            //     s_moveInput = new Vector2(delta.x / m_joystickMaxRadius, delta.y / m_joystickMaxRadius);
            // }
            // else
            // {
            //     m_moveJoystickHandle.localPosition = Vector3.MoveTowards(m_moveJoystickHandle.localPosition, Vector3.zero, Time.unscaledDeltaTime * m_joystickResetSpeed);
            //     s_moveInput = Vector2.zero;
            // }
        }

        private void AimControlsActivation(bool activate)
        {
            for (int i = 0; i < m_controlsToEnableOnlyInAim.Length; i++)
                m_controlsToEnableOnlyInAim[i].SetActive(activate);
        }

        private bool IsOverUI(RectTransform rt, Vector2 position)
        {
            var normalizedMousePosition = new Vector2(position.x / Screen.width, position.y / Screen.height);
            if (normalizedMousePosition.x > rt.anchorMin.x && normalizedMousePosition.x < rt.anchorMax.x &&
                normalizedMousePosition.y > rt.anchorMin.y && normalizedMousePosition.y < rt.anchorMax.y)
            {
                return true;
            }

            return false;
        }
    }

}
