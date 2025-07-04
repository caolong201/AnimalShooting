using IE.RSB;

using System.Collections.Generic;

using UnityEngine;

public class ArrowIndicatorManager : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera mainCamera;
    private List<Enemy> enemies = new List<Enemy>();
    private List<GameObject> arrows = new List<GameObject>();

    private float padding = 50f;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void SetEnemies(List<Enemy> enemyList)
    {
        // Clear cũ
        foreach (var arrow in arrows)
        {
            Destroy(arrow);
        }

        arrows.Clear();
        enemies = enemyList;

        // Tạo mũi tên cho từng enemy
        foreach (var enemy in enemies)
        {
            GameObject arrow = Instantiate(arrowPrefab, canvas.transform);
            arrow.SetActive(false);
            arrows.Add(arrow);
        }
    }

    private void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            UpdateArrow(enemies[i], arrows[i]);
        }
    }

    private void UpdateArrow(Enemy enemy, GameObject arrow)
    {
        if (enemy == null || enemy.isDead)
        {
            arrow.SetActive(false);
            return;
        }

        Vector3 screenPoint = mainCamera.WorldToScreenPoint(enemy.transform.position);
        bool isOffScreen = screenPoint.z < 0 ||
                           screenPoint.x < 0 || screenPoint.x > Screen.width ||
                           screenPoint.y < 0 || screenPoint.y > Screen.height;

        if (!isOffScreen)
        {
            arrow.SetActive(false);
            return;
        }

        arrow.SetActive(true);

        // Nếu sau lưng camera → lật lại
        if (screenPoint.z < 0)
        {
            screenPoint *= -1f;
        }

        screenPoint.x = Mathf.Clamp(screenPoint.x, padding, Screen.width - padding);
        screenPoint.y = Mathf.Clamp(screenPoint.y, padding, Screen.height - padding);

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 dir = (screenPoint - screenCenter).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        arrow.transform.position = screenPoint;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
