using UnityEngine;
using System.Collections;
using IE.RSB;

public class TouchCameraRotation : MonoBehaviour
{
    public float rotateSpeed = 0.05f; // 回転速度
    public float minYAngle = -80f;   // X軸回転の最小角度
    public float maxYAngle = 80f;    // X軸回転の最大角度

    private float rotationX = 0f;
    private float rotationY = 0f;

    [SerializeField] private PlayerWeaponController m_weaponController = null;
    private bool m_isControlsInAimEnabled = false;

    [SerializeField] private GameObject UI_Scope = null;
    [SerializeField] private GameObject UICrossScope = null;
    public bool touchFlg = false;
    
    private const float clickTimeSpanThreshould = 0.5f;
    // ボタンのクリックを受け付けるかどうか
    private bool _clickable = true;
    // カウンタ
    private float _timer = 0f;
    private float _bulletTimer = 0f;
    
    void Start()
    {
        ResetRotation();
        _timer = 1.0f;
    }

    public void ResetRotation(){
        // 現在の回転を初期値として記録
        Vector3 angles = transform.eulerAngles;
        rotationX = 0f;
        rotationY = angles.y;
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

        // Debug.Log("rotationX:"+rotationX);
        // Debug.Log("rotationY:"+rotationY);
    }

    void Update()
    {
        //タッチ操作はTrueの時のみ
        if(touchFlg){
            #if !(!UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS))
                HandleMouseInput(); // PC用の操作
                rotateSpeed = 0.1f;
            #else
                HandleTouchInput(); // モバイル用の操作
            #endif
        }

        if(!m_weaponController.isAiming){
            _timer += Time.deltaTime;
        }else{
            _bulletTimer += Time.deltaTime;
        }
        // {
        //     _timer += Time.deltaTime;
        //     if(_timer > clickTimeSpanThreshould)
        //     {
        //         _clickable = true;
        //         _timer = 0f;
        //     }
        // }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1) // 1本指スワイプで回転
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.deltaPosition.x * rotateSpeed;
                float deltaY = touch.deltaPosition.y * rotateSpeed;

                rotationY += deltaX;
                rotationX -= deltaY;
                rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle); // 上下回転の制限

                // カメラの回転を適用
                transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
            }
            
            if (touch.phase == TouchPhase.Began)
            {
                if(!m_weaponController.isAiming && _timer >= 0.2f){
                    _timer = 0f;
                    ScopeChange();
                    ScopeUIChange();
                    m_weaponController.scopeMode = true;
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if(m_weaponController.isAiming && m_weaponController.scopeMode){
                    m_weaponController.scopeMode = false;
                    m_weaponController.FireInput();
                    StartCoroutine(WaiteScopeChange());
                }
            }
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButton(0)) // 右クリックで回転
        {
            float deltaX = Input.GetAxis("Mouse X") * rotateSpeed * 10f;
            float deltaY = Input.GetAxis("Mouse Y") * rotateSpeed * 10f;

            rotationY += deltaX;
            rotationX -= deltaY;
            rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle); // 上下回転の制限

            // カメラの回転を適用
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("カメラアングル①:"+Camera.main.gameObject.transform.rotation);
            if(!m_weaponController.isAiming && _timer >= 0.2f){
                _timer = 0f;
                ScopeChange();
                ScopeUIChange();
                m_weaponController.scopeMode = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(m_weaponController.isAiming && m_weaponController.scopeMode){
                m_weaponController.scopeMode = false;
                m_weaponController.FireInput();
                StartCoroutine(WaiteScopeChange());
            }
        }
    }

    void ScopeChange(){
        //Debug.Log("カメラアングル②:"+Camera.main.gameObject.transform.rotation);
        m_weaponController.AimInput(!m_isControlsInAimEnabled);

        // Enable aim controls (zoom in&out, zero distance up&down) if aimed in.
        m_isControlsInAimEnabled = !m_isControlsInAimEnabled;
        //AimControlsActivation(m_isControlsInAimEnabled);
    }

    void ScopeUIChange(){
        if(UI_Scope.activeSelf){
            UI_Scope.SetActive(false);
        }else{
            UI_Scope.SetActive(true);
        }
    }

    IEnumerator WaiteScopeChange(){
        if(!m_weaponController.scopeMode){
            yield return new WaitForSecondsRealtime(1.0f);
            if(!SniperAndBallisticsSystem.instance.BulletTimeRunning) ScopeUIChange();
            yield return new WaitUntil(() => !SniperAndBallisticsSystem.instance.BulletTimeRunning);
            ScopeChange();
        }
    }
}