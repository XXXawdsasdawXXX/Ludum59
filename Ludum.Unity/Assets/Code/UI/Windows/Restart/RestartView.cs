using Code.UI.Base;
using UnityEngine;

namespace Code.UI.Windows.Restart
{
    public class RestartView : UIView
    {
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] public Animator TVAnimator { get; private set; }
    }
}