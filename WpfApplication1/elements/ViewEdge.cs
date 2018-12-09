﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApplication1.elements
{
    public class ViewEdge : ViewObject
    {
        public ViewNode StartNode { get; set; }

        public ViewNode EndNode { get; set; }
        
        public ViewEdge(ViewableDataObject UserData,ViewNode StartNode, ViewNode EndNode) {
            if (UserData == null) throw new ArgumentNullException("UserData");
            if (StartNode == null) throw new ArgumentNullException("StartNode");
            if (EndNode == null) throw new ArgumentNullException("EndNode");

            this.UserData = UserData;
            this.StartNode = StartNode;
            this.EndNode = EndNode;

            Drawable = new shapes.ConnectorDrawingObject(
                () =>
                {
                    return this.StartNode.OutgoingConnectorLocation;
                },
                () => {
                    return this.EndNode.IncomingConnectorLocation;
                }
            );

        }

        public shapes.IDrawable Drawable { get; set; }

        #region Overrides

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Drawable.OnRender(drawingContext);
        }


        #endregion
    }
}
