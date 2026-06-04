using UnityEngine;

namespace cats
{
    public interface ITabHandler : IButtonHandler
    {
        GameObject TabPanel { get; }
        void ActivateTab();
        void DisactivateTab();
    }
}
