using DG.Tweening;
using Input;
using UnityEngine;

namespace GameCamera
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private float rotationAmount = 90;
        [SerializeField] private float rotationDuration = 1;

        private Vector3 _endRot;

        private Transform _trans;

        private void Start()
        {
           var playerControls =  new PlayerControls();
           playerControls.Enable();
           
            _trans = gameObject.transform;
        }

        public void RotateLeft()
        {
            if (DOTween.IsTweening(_trans))
                return;
            
            _endRot.y = _trans.rotation.eulerAngles.y + rotationAmount;
            _trans.DOLocalRotate(_endRot, rotationDuration).SetEase(Ease.InOutQuad);
        }

        public void RotateRight()
        {
            if (DOTween.IsTweening(_trans))
                return;
            
            _endRot.y = _trans.rotation.eulerAngles.y - rotationAmount;
            _trans.DOLocalRotate(_endRot, rotationDuration).SetEase(Ease.InOutQuad);
        }
        
    }   
}