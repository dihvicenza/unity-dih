using UnityEngine;

public class TankSuspension : MonoBehaviour
{
    // =====================================================
    // REFERENCES
    // =====================================================

    [Header("References")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform body;
    [SerializeField] Transform[] wheels;

    // =====================================================
    // SUSPENSION PARAMETERS
    // =====================================================

    [Header("Suspension")]
    [SerializeField] float suspensionRestLength = 0.6f;
    [SerializeField] float suspensionTravel = 0.35f;
    [SerializeField] float springStrength = 35000f;
    [SerializeField] float damperStrength = 4500f;

    // =====================================================
    // WHEEL / SPHERECAST PARAMETERS
    // =====================================================

    [Header("Wheel / Ground Detection")]
    [SerializeField] float wheelRadius = 0.35f;
    [SerializeField] float sphereRadius = 0.22f;
    [SerializeField] LayerMask groundMask;

    // =====================================================
    // VISUAL PARAMETERS
    // =====================================================

    [Header("Visual")]
    [SerializeField] bool moveWheelVisuals = true;
    [SerializeField] float visualOffset = 0f;

    // =====================================================
    // DEBUG
    // =====================================================

    [Header("Debug")]
    [SerializeField] bool drawDebug = true;

    // =====================================================
    // INTERNAL DATA
    // =====================================================

    Vector3[] wheelStartLocalPositions;
    float[] previousSuspensionLengths;
    float[] currentSuspensionLengths;

    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        wheelStartLocalPositions = new Vector3[wheels.Length];
        previousSuspensionLengths = new float[wheels.Length];
        currentSuspensionLengths = new float[wheels.Length];

        for (int i = 0; i < wheels.Length; i++)
        {
            wheelStartLocalPositions[i] = wheels[i].localPosition;
            previousSuspensionLengths[i] = suspensionRestLength;
            currentSuspensionLengths[i] = suspensionRestLength;
        }
    }

    // =====================================================
    // FIXED UPDATE
    // =====================================================

    void FixedUpdate()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            SimulateSuspension(i);
        }
    }

    // =====================================================
    // SUSPENSION
    // =====================================================

    void SimulateSuspension(int index)
    {
        Transform wheel = wheels[index];

        if (wheel == null || rb == null || body == null)
            return;

        Vector3 suspensionOrigin =
            body.TransformPoint(wheelStartLocalPositions[index]);

        Vector3 suspensionDirection =
            -body.up;

        float maxSuspensionLength =
            suspensionRestLength + suspensionTravel;

        float castDistance =
            maxSuspensionLength + wheelRadius;

        RaycastHit hit;

        bool hasHit = Physics.SphereCast(
            suspensionOrigin,
            sphereRadius,
            suspensionDirection,
            out hit,
            castDistance,
            groundMask
        );

        if (hasHit)
        {
            float rawLength =
                hit.distance - wheelRadius;

            float minLength =
                suspensionRestLength - suspensionTravel;

            float maxLength =
                suspensionRestLength + suspensionTravel;

            float currentLength =
                Mathf.Clamp(rawLength, minLength, maxLength);

            currentSuspensionLengths[index] = currentLength;

            float compression =
                suspensionRestLength - currentLength;

            float springForce =
                compression * springStrength;

            float suspensionVelocity =
                (previousSuspensionLengths[index] - currentLength) / Time.fixedDeltaTime;

            float damperForce =
                suspensionVelocity * damperStrength;

            float finalForce =
                springForce + damperForce;

            if (finalForce < 0f)
            {
                finalForce = 0f;
            }

            Vector3 force =
                body.up * finalForce;

            rb.AddForceAtPosition(
                force,
                suspensionOrigin,
                ForceMode.Force
            );

            previousSuspensionLengths[index] = currentLength;

            if (moveWheelVisuals)
            {
                Vector3 localPos = wheelStartLocalPositions[index];

                localPos.y =
                    wheelStartLocalPositions[index].y
                    - currentLength
                    + suspensionRestLength
                    + visualOffset;

                wheel.localPosition = localPos;
            }
        }
        else
        {
            float extendedLength =
                suspensionRestLength + suspensionTravel;

            currentSuspensionLengths[index] = extendedLength;
            previousSuspensionLengths[index] = extendedLength;

            if (moveWheelVisuals)
            {
                Vector3 localPos = wheelStartLocalPositions[index];

                localPos.y =
                    wheelStartLocalPositions[index].y
                    - suspensionTravel
                    + visualOffset;

                wheel.localPosition = localPos;
            }
        }
    }

    // =====================================================
    // DEBUG
    // =====================================================

    void OnDrawGizmos()
    {
        if (!drawDebug)
            return;

        if (body == null || wheels == null)
            return;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i] == null)
                continue;

            Vector3 origin;

            if (wheelStartLocalPositions != null &&
                wheelStartLocalPositions.Length == wheels.Length)
            {
                origin = body.TransformPoint(wheelStartLocalPositions[i]);
            }
            else
            {
                origin = wheels[i].position;
            }

            Vector3 direction = -body.up;

            float maxSuspensionLength =
                suspensionRestLength + suspensionTravel;

            Vector3 end =
                origin + direction * maxSuspensionLength;

            Gizmos.DrawLine(origin, end);
            Gizmos.DrawWireSphere(end, sphereRadius);
        }
    }
}