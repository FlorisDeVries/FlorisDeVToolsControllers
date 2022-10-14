using System;
using System.Collections.Generic;
using FlorisDeVToolsFSM.UnityExtensions;
using FlorisDeVToolsMathLibrary;
using FlorisDeVToolsUnityExtensions.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlorisDeVToolsControllers.Environment.Platforms
{
    public enum FollowType
    {
        PingPong,
        Circular
    }

    public class WaypointPlatformController : GameBehaviour
    {
        [Tooltip("In meters/second")] [SerializeField]
        private float _moveSpeed = 2f;

        [SerializeField] private Transform _platform;
        [SerializeField] private Transform _wayPointsParent;
        [SerializeField] private FollowType _followType = FollowType.Circular;

        private Rigidbody _rigidbody;
        private Vector3 _targetPosition;
        private Vector3 _startPosition;

        private List<Transform> _wayPoints;
        private int _currentIndex = 0;

        private float _currentDist;
        private float _timeToCoverDistance;
        private float _timer;

        private bool _returning = false;


        private void OnEnable()
        {
            _rigidbody = _platform.GetComponent<Rigidbody>();
            if (!_rigidbody)
            {
                _rigidbody = _platform.gameObject.AddComponent<Rigidbody>();
            }

            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;

            _wayPoints = _wayPointsParent.GetChildren();
            NextWayPoint(true);
        }

        private void FixedUpdate()
        {
            if(IsPaused)
                return;

            if (_wayPoints.Count < 2)
                return;
            
            var percentage =
                EasingFunction.EaseInOutSine(0, 1, _timer / _timeToCoverDistance);
            _timer += Time.fixedDeltaTime;

            if (percentage > .99f)
            {
                NextWayPoint();
            }
            else
            {
                var targetPosition = Vector3.Lerp(_startPosition, _targetPosition, percentage);
                _rigidbody.MovePosition(targetPosition);
            }
        }

        private void NextWayPoint(bool initial = false)
        {
            if (_wayPoints.Count < 2)
                return;

            _startPosition = _platform.position;
            _timer = 0f;

            UpdateIndex(initial);

            _targetPosition = _wayPoints[_currentIndex].position;
            _currentDist = (_targetPosition - _platform.position).magnitude;
            _timeToCoverDistance = _currentDist / _moveSpeed;
        }

        private void UpdateIndex(bool initial = false)
        {
            if (initial)
            {
                _currentIndex = 1;
                return;
            }

            switch (_followType)
            {
                case FollowType.Circular:
                {
                    _currentIndex++;
                    if (_currentIndex >= _wayPoints.Count) _currentIndex = 0;
                    break;
                }
                case FollowType.PingPong when !_returning:
                    _currentIndex++;
                    if (_currentIndex >= _wayPoints.Count)
                    {
                        _currentIndex -= 2;
                        _returning = true;
                    }
                    break;
                case FollowType.PingPong when _returning:
                    _currentIndex--;
                    if (_currentIndex < 0)
                    {
                        _currentIndex += 2;
                        _returning = false;
                    }
                    break;
                default:
                    _currentIndex++;
                    break;
            }
        }

        [Button]
        private void AddWayPoint()
        {
            var gO = new GameObject($"Waypoint {_wayPointsParent.childCount + 1}");
            gO.transform.SetParent(_wayPointsParent);
            gO.transform.position = _platform.position;
        }
    }
}