using HarmonyLib;
using UnityEngine;

namespace SeaAnimals
{
    [HarmonyPatch]
    public static class MarineLifeMountControl
    {
        public static float currentTurnInput = 0f;
    }

    [HarmonyPatch(typeof(Player), nameof(Player.StartDoodadControl))]
    public static class Player_StartDoodadControl_MarineLife
    {
        public static Humanoid RidingHumanoid;
        static bool Prefix(Player __instance, IDoodadController shipControl)
        {
            if (shipControl == null) return true;
            Humanoid mount = shipControl.GetControlledComponent()?.transform?.GetComponentInParent<Humanoid>();
            if (mount == null || !Utils.GetPrefabName(mount.gameObject.name).StartsWith("SA_")) return true;
            RidingHumanoid = mount;
            return true;
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.StopDoodadControl))]
    public static class Player_StopDoodadControl_MarineLife
    {
        static void Postfix() { Player_StartDoodadControl_MarineLife.RidingHumanoid = null; }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.Update))]
    public static class Player_Update_Input_MarineLife
    {
        static void Prefix(Player __instance)
        {
            if (__instance != Player.m_localPlayer || !__instance.IsRiding() || Player_StartDoodadControl_MarineLife.RidingHumanoid == null) return;

            Humanoid mount = Player_StartDoodadControl_MarineLife.RidingHumanoid;
            string prefabName = Utils.GetPrefabName(mount.gameObject.name);

            // Desmonte automático (Trava de Terra)
            if (!prefabName.Contains("Crocodile"))
            {
                if (mount.IsOnGround() && !mount.IsSwimming())
                {
                    __instance.StopDoodadControl();
                    return;
                }
            }

            float horizontal = 0f;
            if (ZInput.GetButton("Left")) horizontal -= 1f;
            if (ZInput.GetButton("Right")) horizontal += 1f;
            MarineLifeMountControl.currentTurnInput = horizontal;
        }
    }

    [HarmonyPatch(typeof(Sadle), "ApplyControlls")]
    public static class Sadle_ApplyControlls_Movement_Patch
    {
        static bool Prefix(Sadle __instance, Vector3 moveDir, Vector3 lookDir, bool run)
        {
            Humanoid ridingHumanoid = Player_StartDoodadControl_MarineLife.RidingHumanoid;
            if (ridingHumanoid == null || __instance.m_character != ridingHumanoid) return true;

            if (__instance.m_monsterAI != null) __instance.m_monsterAI.StopAllCoroutines();

            float turnInput = MarineLifeMountControl.currentTurnInput;
            bool isMovingForward = Mathf.Abs(moveDir.z) > 0.1f;

            // Animação (Look Dir)
            Vector3 lookVector = ridingHumanoid.transform.forward;
            if (Mathf.Abs(turnInput) > 0.01f)
            {
                lookVector = (ridingHumanoid.transform.forward + ridingHumanoid.transform.right * turnInput * 0.5f).normalized;
            }
            ridingHumanoid.SetLookDir(lookVector);

            // Física e Rotação
            Vector3 finalMoveVec = Vector3.zero;
            Sadle.Speed speed = Sadle.Speed.Stop;

            if (isMovingForward)
            {
                ridingHumanoid.transform.Rotate(0f, turnInput * 2.0f, 0f);
                finalMoveVec = ridingHumanoid.transform.forward * Mathf.Sign(moveDir.z);
                speed = run ? Sadle.Speed.Run : Sadle.Speed.Walk;
            }
            else if (Mathf.Abs(turnInput) > 0.01f)
            {
                ridingHumanoid.transform.Rotate(0f, turnInput * 1.5f, 0f);
                finalMoveVec = ridingHumanoid.transform.right * turnInput * 0.01f;
                speed = Sadle.Speed.Walk;
            }

            float rideSkill = Player.m_localPlayer.GetSkills().GetSkillFactor(Skills.SkillType.Ride);
            __instance.m_nview.InvokeRPC("Controls", finalMoveVec, (int)speed, rideSkill);

            return false;
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.SetControls))]
    public static class Player_SetControls_ActionRedirect_Patch
    {
        static void Prefix(Player __instance, ref bool jump, ref bool attack, ref bool secondaryAttack)
        {
            Humanoid mount = Player_StartDoodadControl_MarineLife.RidingHumanoid;
            if (__instance.IsRiding() && mount != null)
            {
                attack = false;
                secondaryAttack = false;

                if (jump)
                {
                    // VERIFICAÇÃO: Se não for o crocodilo, pula.
                    if (!Utils.GetPrefabName(mount.gameObject.name).Contains("Crocodile"))
                    {
                        mount.Jump();
                        if (mount.m_animator != null) mount.m_animator.SetTrigger("jump");
                    }
                }
                jump = false;
            }
        }
    }

    [HarmonyPatch(typeof(Player), "Update")]
    public static class Player_Update_MountAttack_MarineLife
    {
        static void Postfix(Player __instance)
        {
            Humanoid mount = Player_StartDoodadControl_MarineLife.RidingHumanoid;
            if (mount == null || __instance != Player.m_localPlayer || !__instance.TakeInput()) return;
            if (!mount.InAttack())
            {
                if (ZInput.GetButtonDown("Attack")) ProcessAttack(mount, 0);
                if (ZInput.GetButtonDown("SecondaryAttack")) ProcessAttack(mount, 1);
            }
        }
        private static void ProcessAttack(Humanoid mount, int index)
        {
            if (mount.m_defaultItems != null && mount.m_defaultItems.Length > index)
            {
                mount.m_rightItem = mount.m_defaultItems[index].GetComponent<ItemDrop>().m_itemData;
                mount.StartAttack(null, false);
            }
        }
    }
}