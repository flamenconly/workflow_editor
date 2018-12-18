using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1.elements
{
    internal class FrameworkHelper : IFrameworkHelper
    {
        private static IFrameworkHelper Instance;

        public static IFrameworkHelper GetInstance() {

            if (Instance == null) Instance = new FrameworkHelper();
            return Instance;

        }

        public Rect DetermineBoundingBoxOfUIItemWithinCanvas(FrameworkElement child) {

            var top = Canvas.GetTop(child);
            var left = Canvas.GetLeft(child);
            var bottom = top + child.DesiredSize.Height;
            var right = left + child.DesiredSize.Width;

            var boundingBox = new Rect(
                new Point(left, top), new Size(child.DesiredSize.Width, child.DesiredSize.Height)
                );

            return boundingBox;

        }

        /// <summary>
        /// Checks if targetPosition is within the dimension of the boundingBox
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <param name="boundingBox"></param>
        /// <returns></returns>
        public bool IsPositionWithinBoundingBox(Point targetPosition, Rect boundingBox) {
            return boundingBox.Contains(targetPosition);
        }
    }
}
