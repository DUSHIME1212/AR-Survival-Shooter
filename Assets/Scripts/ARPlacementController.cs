// Scripts/AR/ARPlacementController.cs
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

namespace ARSurvivalShooter
{
    public class ARPlacementController : MonoBehaviour
    {
        public static ARPlacementController Instance { get; private set; }

        [SerializeField] private GameObject gameWorldPrefab;
        [SerializeField] private ARRaycastManager raycastManager;
        [SerializeField] private ARPlaneManager planeManager;
        [SerializeField] private GameObject placementIndicator; // a flat disc showing where you'll place

        private GameObject spawnedGameWorld;
        private List<ARRaycastHit> hits = new List<ARRaycastHit>();
        private bool gamePlaced = false;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Update()
        {
            if (gamePlaced || GameManager.Instance.CurrentState != GameManager.GameState.PlacingGame) return;

            // Cast ray from screen center
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

            if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinBounds))
            {
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(hits[0].pose.position, hits[0].pose.rotation);
            }
            else
            {
                placementIndicator.SetActive(false);
            }
        }

        // Called by a "Place Game" button in the UI
        public void PlaceGame()
        {
            if (hits.Count == 0) return;

            Pose hitPose = hits[0].pose;
            spawnedGameWorld = Instantiate(gameWorldPrefab, hitPose.position, hitPose.rotation);

            // Attach a world anchor so it stays in real-world position
            spawnedGameWorld.AddComponent<ARAnchor>();

            // Tell EnemySpawner where to spawn enemies
            EnemySpawner.Instance.Initialize(spawnedGameWorld.transform);

            // Hide plane visualizers after placement
            foreach (var plane in planeManager.trackables)
                plane.gameObject.SetActive(false);
            planeManager.enabled = false;

            gamePlaced = true;
            GameManager.Instance.StartGame();
        }
    }
}