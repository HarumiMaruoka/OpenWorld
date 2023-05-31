// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyState_WaypointWalk : MonoBehaviour
{
    private CharacterController _characterController = null;
    private bool _isMoving = false;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {

    }

    public async void Move(Vector3 position, float releaseSquareDistance = 0.01f, CancellationToken token = default, Action onComplete = default)
    {
        if (_isMoving) return;
        _isMoving = true;
        if (releaseSquareDistance < 0.01f) releaseSquareDistance = 0.01f;
        while ((position - transform.position).sqrMagnitude > releaseSquareDistance * releaseSquareDistance && !token.IsCancellationRequested)
        {
            Vector3 direction = (position - transform.position).normalized;
            _characterController.Move(direction * Time.deltaTime);
            await UniTask.Yield();
        }
        _isMoving = false;
        onComplete?.Invoke();
    }

    public async void Move(Vector3[] positions, float incrementSquareDistance = 0.01f, CancellationToken token = default, Action onComplete = default)
    {
        if (_isMoving) return;
        _isMoving = true;
        int currentIndex = 0;
        if (incrementSquareDistance < 0.01f) incrementSquareDistance = 0.01f;
        while (currentIndex < positions.Length && !token.IsCancellationRequested)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = positions[currentIndex];
            while ((targetPosition - currentPosition).sqrMagnitude > incrementSquareDistance * incrementSquareDistance && currentIndex < positions.Length && !token.IsCancellationRequested)
            {
                Vector3 direction = (targetPosition - currentPosition).normalized;
                _characterController.Move(direction * Time.deltaTime);
                currentPosition = transform.position;
                await UniTask.Yield();
            }
            currentIndex++;
        }
        _isMoving = false;
        onComplete?.Invoke();
    }

    public async void Move(List<Vector3> positions, float incrementSquareDistance = 0.01f, CancellationToken token = default, Action onComplete = default)
    {
        if (_isMoving) return;
        _isMoving = true;
        int currentIndex = 0;
        if (incrementSquareDistance < 0.01f) incrementSquareDistance = 0.01f;
        while (currentIndex < positions.Count && !token.IsCancellationRequested)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = positions[currentIndex];
            while ((targetPosition - currentPosition).sqrMagnitude > incrementSquareDistance * incrementSquareDistance && currentIndex < positions.Count && !token.IsCancellationRequested)
            {
                Vector3 direction = (targetPosition - currentPosition).normalized;
                _characterController.Move(direction * Time.deltaTime);
                currentPosition = transform.position;
                await UniTask.Yield();
            }
            currentIndex++;
        }
        _isMoving = false;
        onComplete?.Invoke();
    }
}
