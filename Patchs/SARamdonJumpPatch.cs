using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeaAnimals
{
    public class CreatureJump : MonoBehaviour
    {
        private Animator animator;
        private Character character;

        private readonly List<string> allowedCreatures = new List<string> { "SA_Orca", "SA_Dolphin" };
        private bool isAllowed = false;

        private void Start()
        {
            character = GetComponent<Character>();

            string prefabName = Utils.GetPrefabName(gameObject.name);
            if (allowedCreatures.Contains(prefabName))
            {
                isAllowed = true;
            }

            Transform visualTransform = transform.Find("Visual");
            if (visualTransform != null)
            {
                animator = visualTransform.GetComponent<Animator>();
            }

            if (isAllowed)
            {
                StartCoroutine(JumpRoutine());
            }
        }

        private IEnumerator JumpRoutine()
        {
            while (true)
            {
                float waitTime = Random.Range(30f, 60f);
                yield return new WaitForSeconds(waitTime);

                if (animator != null && character != null)
                {

                    bool isTamed = character.IsTamed();
                    bool isBeingRidden = character.IsAttached();
                    bool isOwner = character.m_nview != null && character.m_nview.IsOwner();

                    if (!isTamed && !isBeingRidden && isOwner)
                    {
                        Debug.Log($"[SeaAnimals] Pulando (Selvagem): {gameObject.name}.");
                        animator.SetTrigger("jump");
                    }
                }
            }
        }
    }
}