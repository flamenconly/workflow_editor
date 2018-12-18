using System.Windows;

namespace WpfApplication1.elements
{
    internal interface IFrameworkHelper
    {
        Rect DetermineBoundingBoxOfUIItemWithinCanvas(FrameworkElement child);
        bool IsPositionWithinBoundingBox(Point targetPosition, Rect boundingBox);
    }
}