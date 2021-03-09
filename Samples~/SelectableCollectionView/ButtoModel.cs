using UnityEngine;
using UnityMVVM.Model;

namespace UnityMVVM.Samples.SelectableCollectionView
{
    public class ButtonModel : ModelBase
    {
        public Color color { get; set; }
        public string label { get; set; }
    }
}