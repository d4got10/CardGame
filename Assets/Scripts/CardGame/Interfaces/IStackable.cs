using UnityEngine;

namespace CardGame
{
    public interface IStackable
    {
        Vector3 GetStackedWorldPosition();
        bool CanBeGrabbed();
        void ForcedStack(Card card);
        bool Stack(Card card);
        void UnStack(IStackable newParent);
    }
}
