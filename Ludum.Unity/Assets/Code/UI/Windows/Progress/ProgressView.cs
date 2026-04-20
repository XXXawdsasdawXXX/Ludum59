using Code.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows.Progress
{
    public class ProgressView : UIView
    {
        [field: SerializeField] public Image[] Stages { get; private set; }
    }
}