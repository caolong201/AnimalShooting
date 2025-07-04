
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController: MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image crosshairImage;           
    [SerializeField] private Transform firePoint;               

    [Header("Settings")]
    [SerializeField] private LayerMask enemyLayer;              
    [SerializeField] private float detectionRange = 100f;       
    [SerializeField] private Color defaultColor = Color.white;  
    [SerializeField] private Color highlightColor = Color.red;  
    [SerializeField] private float transitionSpeed = 10f;       

    private Color currentColor;

    private void Start()
    {
        if (crosshairImage == null)
            Debug.LogError("⚠️ Crosshair Image chưa được gán!");

        currentColor = defaultColor;
        crosshairImage.color = currentColor;
    }

    private void Update()
    {
        UpdateCrosshairColor();
    }

    private void UpdateCrosshairColor()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        Color targetColor = defaultColor;

        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange, enemyLayer))
        {
            targetColor = highlightColor;
        }

        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * transitionSpeed);
        crosshairImage.color = currentColor;
    }
}
