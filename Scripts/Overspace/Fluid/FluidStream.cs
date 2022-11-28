using System.Collections;
using Overspace.Fluid.Container;
using UnityEngine;

namespace Overspace.Fluid
{
    public class FluidStream : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public ParticleSystem particle;
        public float animationSpeed = 1.75f;
        public float targetOffset = 0.025f;
        public float addAmount = 0.1f;

        private Coroutine pourRoutine;
        private Vector3 targetPosition = Vector3.zero;

        private FluidData data;

        private Collider lastHit;

        private void Start()
        {
            Vector3 currentPosition = transform.position;
            MoveToPosition(0, currentPosition);
            MoveToPosition(1, currentPosition);
        }

        public void Begin()
        {
            StartCoroutine(UpdateHitting());
            pourRoutine = StartCoroutine(BeginPour());
        }

        private IEnumerator BeginPour()
        {
            while (gameObject.activeSelf)
            {
                targetPosition = FindEndPoint();
                MoveToPosition(0, transform.position);
                AnimateToPosition(1, targetPosition);
                yield return null;
            }
        }

        public void End()
        {
            StopCoroutine(pourRoutine);
            pourRoutine = StartCoroutine(EndPour());
        }

        private IEnumerator EndPour()
        {
            while (!HasReachedPosition(0, targetPosition))
            {
                AnimateToPosition(0, targetPosition);
                AnimateToPosition(1, targetPosition);
                yield return null;
            }

            FluidManager.Instance.DestroyStream(this);
        }

        private Vector3 FindEndPoint()
        {
            Ray ray = new(transform.position, Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit, 2.0f);
            Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);

            lastHit = hit.collider ? hit.collider : null;
            
            return endPoint + Vector3.up * targetOffset;
        }

        private void MoveToPosition(int index, Vector3 position)
        {
            lineRenderer.SetPosition(index, position);
        }

        private void AnimateToPosition(int index, Vector3 position)
        {
            Vector3 currentPos = lineRenderer.GetPosition(index);
            Vector3 newPos = Vector3.MoveTowards(currentPos, position, Time.deltaTime * animationSpeed);
            lineRenderer.SetPosition(index, newPos);
        }

        private bool HasReachedPosition(int index, Vector3 target)
        {
            Vector3 currentPos = lineRenderer.GetPosition(index);
            return currentPos == target;
        }

        private IEnumerator UpdateHitting()
        {
            while (gameObject.activeSelf)
            {
                particle.gameObject.transform.position = targetPosition;
                particle.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.up);

                bool isHitting = HasReachedPosition(1, targetPosition);

                if (isHitting && lastHit)
                {
                    if (lastHit.CompareTag("Shaker"))
                    {
                        if (lastHit.transform.root.TryGetComponent(out FluidShakerContainer shaker))
                        {
                            shaker.AddFluid(data, addAmount * Time.deltaTime);
                        }
                    }
                }
                
                if (isHitting && !particle.isPlaying) particle.Play();
                else if (!isHitting && particle.isPlaying) particle.Stop();
            
                yield return null;
            }
        }

        public void SetData(FluidData newData)
        {
            data = newData;
            Color color = data.fluidColor;
            
            lineRenderer.startColor = new Color(color.r, color.g, color.b, 0f);
            lineRenderer.endColor = color;
            ParticleSystem.MainModule main = particle.main;
            main.startColor = color;
        }
    }
}