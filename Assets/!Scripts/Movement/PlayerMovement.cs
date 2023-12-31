using Shir0.InputSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shir0.Movement
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerMovement : MonoBehaviour, IPlayerInputListener
    {
        #region Input Listener Data
        private PlayerInputHandler m_targetSender;
        public string ActionName
        {
                get { return "Move"; }
        }
        public PlayerInputHandler PreferredSender
        {
            get
            {
                return m_targetSender = m_targetSender != null ? m_targetSender : GetComponent<PlayerInputHandler>();
            }
        }
        #endregion

        [SerializeField] private MovementData m_movementData;

        private Vector2 m_targetDirection = Vector2.zero;
        private bool m_isMoving = false;

        private void Awake()
        {
            m_targetSender = GetComponent<PlayerInputHandler>();
        }

        private void OnEnable()
        {
            PlayerInputHandler.OnInputReceived += PerformAction;
        }

        private void OnDisable()
        {
            PlayerInputHandler.OnInputReceived -= PerformAction;
        }

        public void PerformAction(object sender, PlayerInputHandler.InputEventArgs args)
        {
            if (!sender.Equals(m_targetSender)) return;
            else if (args.ActionName != ActionName) return;

            m_isMoving = !m_isMoving;
            if (m_isMoving)
                StartCoroutine(MoveCoroutine(args.Context.action));
        }

        IEnumerator MoveCoroutine(InputAction action)
        {
            m_isMoving = true;
            
            Vector2 direction = action.ReadValue<Vector2>().normalized;

            while (direction.sqrMagnitude > 0)
            {
                direction = action.ReadValue<Vector2>().normalized;
                transform.position += m_movementData.Acceleration * Time.fixedDeltaTime * (Vector3)direction;

                yield return new WaitForFixedUpdate();
            }
        }

        [System.Serializable]
        private struct MovementData
        {
            [SerializeField] private float m_acceleration;
            [SerializeField] private float m_deceleration;
            [SerializeField] private float m_maxSpeed;

            public readonly float Acceleration => m_acceleration;
            public readonly float Deceleration => m_deceleration;
            public readonly float MaxSpeed => m_maxSpeed;
        }
    }
}