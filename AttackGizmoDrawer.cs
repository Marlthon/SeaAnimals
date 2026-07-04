using UnityEngine;

#if UNITY_EDITOR
public class AttackGizmoDrawer : MonoBehaviour
{
    [Header("Attack (ItemDrop.Attack)")]
    public Attack attack;

    public Color rangeColor = Color.cyan;
    public Color coneColor = Color.yellow;
    public Color yAngleColor = Color.magenta;
    public Color rayWidthColor = new Color(1f, 0.5f, 0f);
    public Color hitPointColor = Color.red;

    private void OnDrawGizmos()
    {
        if (attack == null)
            return;

        Transform t = transform;

        // =========================
        // ORIGEM DO ATAQUE
        // =========================
        Vector3 origin = t.position + Vector3.up * attack.m_attackHeight;

        Vector3 attackPoint =
            origin +
            t.forward * (attack.m_attackRange + attack.m_attackOffset);

        // =========================
        // RANGE
        // =========================
        Gizmos.color = rangeColor;
        Gizmos.DrawLine(origin, attackPoint);

        Gizmos.color = hitPointColor;
        Gizmos.DrawSphere(attackPoint, 0.15f);

        // =========================
        // CONE HORIZONTAL
        // =========================
        float halfAngle = attack.m_attackAngle * 0.5f;

        Vector3 leftDir = Quaternion.Euler(0f, -halfAngle, 0f) * t.forward;
        Vector3 rightDir = Quaternion.Euler(0f, halfAngle, 0f) * t.forward;

        Gizmos.color = coneColor;
        Gizmos.DrawLine(origin, origin + leftDir * attack.m_attackRange);
        Gizmos.DrawLine(origin, origin + rightDir * attack.m_attackRange);

        // =========================
        // MAX Y ANGLE (VERTICAL)
        // =========================
        if (attack.m_maxYAngle > 0f)
        {
            float yHalf = attack.m_maxYAngle * 0.5f;

            Vector3 upDir = Quaternion.Euler(-yHalf, 0f, 0f) * t.forward;
            Vector3 downDir = Quaternion.Euler(yHalf, 0f, 0f) * t.forward;

            Gizmos.color = yAngleColor;
            Gizmos.DrawLine(origin, origin + upDir * attack.m_attackRange);
            Gizmos.DrawLine(origin, origin + downDir * attack.m_attackRange);
        }

        // =========================
        // ATTACK RAY WIDTH
        // =========================
        if (attack.m_attackRayWidth > 0f)
        {
            Vector3 side = t.right * (attack.m_attackRayWidth * 0.5f);

            Gizmos.color = rayWidthColor;

            Gizmos.DrawLine(
                origin + side,
                origin + side + t.forward * attack.m_attackRange
            );

            Gizmos.DrawLine(
                origin - side,
                origin - side + t.forward * attack.m_attackRange
            );
        }
    }
}
#endif
