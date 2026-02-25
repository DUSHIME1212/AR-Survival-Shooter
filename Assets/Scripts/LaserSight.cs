// Scripts/Player/LaserSight.cs
using UnityEngine;

namespace ARSurvivalShooter
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserSight : MonoBehaviour
    {
        [SerializeField] private float maxDistance = 50f;
        [SerializeField] private LayerMask ignoreLayers;

        private LineRenderer lineRenderer;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            
            // Set some default laser looks
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.01f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = new Color(1, 0, 0, 0); // fade out
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            {
                lineRenderer.enabled = false;
                return;
            }

            lineRenderer.enabled = true;
            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;

            // Combine ignore layers with safety layers (Projectile, Player, etc.)
            int playerLayer = LayerMask.NameToLayer("Player");
            int projLayer = LayerMask.NameToLayer("Projectile");
            
            // We do NOT exclude "Character" here, as enemies might be on it.
            int mask = ~ignoreLayers.value;
            if (playerLayer != -1) mask &= ~(1 << playerLayer);
            if (projLayer != -1) mask &= ~(1 << projLayer);

            Ray ray = new Ray(origin, direction);
            RaycastHit hit;

            lineRenderer.SetPosition(0, origin);

            if (Physics.Raycast(ray, out hit, maxDistance, mask))
            {
                lineRenderer.SetPosition(1, hit.point);
                // Optional: Log hit for debugging (spams a lot, so maybe keep commented)
                // Debug.Log($"[LaserSight] Hitting: {hit.collider.gameObject.name}");
            }
            else
            {
                lineRenderer.SetPosition(1, origin + direction * maxDistance);
            }
        }
    }
}
